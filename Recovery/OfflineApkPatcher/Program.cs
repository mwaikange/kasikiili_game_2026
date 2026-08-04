using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO.Compression;

if (args.Length == 4 && args[0] == "--inject-apk")
{
    InjectApk(args[1], args[2], args[3]);
    return 0;
}

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: OfflineApkPatcher <input Scripts.dll> <output Scripts.dll> [--dump]");
    return 2;
}

var input = Path.GetFullPath(args[0]);
var output = Path.GetFullPath(args[1]);
var resolver = new DefaultAssemblyResolver();
resolver.AddSearchDirectory(Path.GetDirectoryName(input)!);
using var module = ModuleDefinition.ReadModule(input, new ReaderParameters { AssemblyResolver = resolver, ReadWrite = false });

if (args.Contains("--dump"))
{
    foreach (var type in AllTypes(module))
    {
        foreach (var method in type.Methods.Where(m => m.HasBody && ShouldDump(type, m)))
        {
            Console.WriteLine($"TYPE {type.FullName} METHOD {method.Name}");
            foreach (var instruction in method.Body.Instructions)
                Console.WriteLine($"    {instruction.Offset:X4}: {instruction.OpCode} {instruction.Operand}");
        }
    }
    return 0;
}

PatchLocalWallet(module);
PatchRoundLimit(module);
PatchRouletteStartupActive(module);
PatchOfflineConnection(module);
PatchOfflineLogin(module);
PatchLoginButton(module);
PatchOpenMenu(module);
PatchProbability(module);
PatchNoOp(module, "APIManager", "CallFcmAPI");
PatchJsonApi(
    module,
    "CallLeaderboardAPI",
    "APIManager/<GetLeaderboardResponse>d__12",
    "leaderboard",
    "{\"data\":{\"active_referrals\":\"245\",\"player_position\":\"148\",\"candidates\":[{\"rank\":\"1\",\"mobile_number\":\"26481 XXX 2569\",\"score\":97.0,\"is_upgraded\":true},{\"rank\":\"2\",\"mobile_number\":\"26481 XXX 2569\",\"score\":92.0,\"is_upgraded\":false},{\"rank\":\"3\",\"mobile_number\":\"26481 XXX 2569\",\"score\":88.0,\"is_upgraded\":false},{\"rank\":\"4\",\"mobile_number\":\"26481 XXX 2569\",\"score\":76.0,\"is_upgraded\":true},{\"rank\":\"5\",\"mobile_number\":\"26481 XXX 2569\",\"score\":75.0,\"is_upgraded\":false},{\"rank\":\"6\",\"mobile_number\":\"26481 XXX 2569\",\"score\":69.0,\"is_upgraded\":true},{\"rank\":\"7\",\"mobile_number\":\"26481 XXX 2569\",\"score\":35.0,\"is_upgraded\":false},{\"rank\":\"8\",\"mobile_number\":\"26481 XXX 2569\",\"score\":30.0,\"is_upgraded\":false},{\"rank\":\"9\",\"mobile_number\":\"26481 XXX 2569\",\"score\":26.0,\"is_upgraded\":false},{\"rank\":\"10\",\"mobile_number\":\"26481 XXX 2569\",\"score\":18.0,\"is_upgraded\":false}],\"player\":{\"rank\":\"148\",\"mobile_number\":\"081 XXX 0000\",\"score\":2.0,\"is_upgraded\":true}}}");
PatchJsonApi(
    module,
    "CallPrizeDistributionAPI",
    "APIManager/<GetPrizeDistributionResponse>d__14",
    "prizeDistributionData",
    "{\"data\":[{\"price_amount\":2000,\"position\":\"01\"},{\"price_amount\":1000,\"position\":\"02\"},{\"price_amount\":600,\"position\":\"03\"},{\"price_amount\":200,\"position\":\"04-10\"}]}");

PatchStringIterator(module, "Infrastructure.PlayerGateway/<GetBalance>d__2", "[{\"balance\":500}]");
PatchStringIterator(module, "Infrastructure.PlayerGateway/<UpdateUserBalance>d__10", "Offline balance saved");
PatchStringIterator(module, "Infrastructure.PlayerGateway/<Cashout>d__4", "Successfully");
PatchStringIterator(module, "Infrastructure.PlayerGateway/<CashoutHistoryList>d__6", "[]");
PatchStringIterator(module, "Infrastructure.PlayerGateway/<CashoutAwaiting>d__8", "{\"awaitingcashout\":\"0\"}");
PatchStringIterator(module, "Infrastructure.UserGateway/<PostUserLogin>d__2", "{\"user_id\":1,\"accesstoken\":\"offline\",\"referral_link\":\"offline\",\"mobile_number\":\"0810000000\",\"region\":\"KHOMAS\"}");
PatchStringIterator(module, "Infrastructure.UserGateway/<PostUserSignUp>d__6", "Successfully");
PatchStringIterator(module, "Infrastructure.UserGateway/<CheckMobileNumber>d__8", "Successfully");
PatchStringIterator(module, "Infrastructure.UserGateway/<SendOtp>d__10", "Successfully");
PatchStringIterator(module, "Infrastructure.UserGateway/<VerifyOtp>d__12", "Successfully");
PatchStringIterator(module, "Infrastructure.UserGateway/<ChangePassword>d__14", "Successfully");
PatchStringIterator(module, "Infrastructure.UserGateway/<RegisterOtp>d__16", "Successfully");
PatchStringIterator(module, "Infrastructure.GameGateway/<GetAllUserBalance>d__2", "{\"total_available\":500}");

Directory.CreateDirectory(Path.GetDirectoryName(output)!);
module.Write(output);
Console.WriteLine($"Wrote {output}");
return 0;

static IEnumerable<TypeDefinition> AllTypes(ModuleDefinition module)
{
    foreach (var type in module.Types)
    {
        yield return type;
        foreach (var nested in Nested(type))
            yield return nested;
    }
}

static IEnumerable<TypeDefinition> Nested(TypeDefinition type)
{
    foreach (var nested in type.NestedTypes)
    {
        yield return nested;
        foreach (var child in Nested(nested))
            yield return child;
    }
}

static bool ShouldDump(TypeDefinition type, MethodDefinition method)
{
    return (type.FullName == "Components.GameStartInput" && method.Name == "OnGameStart")
        || (type.FullName == "ViewModel.CashManager" && method.Name == "ResetMoney")
        || (type.FullName == "Components.RoundLimitInput" && method.Name == "Awake")
        || (type.FullName == "APIManager" && (method.Name == "CallProbabilityAPI" || method.Name == "CallFcmAPI"))
        || (type.FullName.Contains("PlayerGateway/<GetBalance>") && method.Name == "MoveNext")
        || (type.FullName.Contains("PlayerGateway/<UpdateUserBalance>") && method.Name == "MoveNext")
        || (type.FullName.Contains("UserGateway/<PostUserLogin>") && method.Name == "MoveNext");
}

static TypeDefinition FindType(ModuleDefinition module, string fullName)
{
    return AllTypes(module).Single(t => t.FullName == fullName);
}

static MethodDefinition FindMethod(ModuleDefinition module, string typeName, string methodName)
{
    return FindType(module, typeName).Methods.Single(m => m.Name == methodName);
}

static void ResetBody(MethodDefinition method)
{
    method.Body.ExceptionHandlers.Clear();
    method.Body.Variables.Clear();
    method.Body.Instructions.Clear();
    method.Body.InitLocals = false;
}

static void PatchLocalWallet(ModuleDefinition module)
{
    var type = FindType(module, "ViewModel.CashManager");
    var method = type.Methods.Single(m => m.Name == "ResetMoney");
    var setter = method.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(m => m.Name == "set_Value" && m.DeclaringType.FullName.StartsWith("UniRx.ReactiveProperty`1", StringComparison.Ordinal));
    var il = method.Body.GetILProcessor();
    ResetBody(method);

    foreach (var (fieldName, value) in new[] { ("currentBet", 0), ("currentCredit", 500), ("currentTub", 0) })
    {
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldfld, type.Fields.Single(f => f.Name == fieldName)));
        il.Append(il.Create(OpCodes.Ldc_I4, value));
        il.Append(il.Create(OpCodes.Callvirt, setter));
    }
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine("Patched local wallet with 500 credits.");
}

static void PatchRoundLimit(ModuleDefinition module)
{
    var owner = FindType(module, "Components.RoundLimitInput");
    var manager = FindType(module, "ViewModel.RouletteManager");
    var awake = owner.Methods.Single(m => m.Name == "Awake");
    var il = awake.Body.GetILProcessor();
    var first = awake.Body.Instructions[0];
    il.InsertBefore(first, il.Create(OpCodes.Ldarg_0));
    il.InsertBefore(first, il.Create(OpCodes.Ldfld, owner.Fields.Single(f => f.Name == "rouletteManager")));
    il.InsertBefore(first, il.Create(OpCodes.Ldc_I4, 1_000_000));
    il.InsertBefore(first, il.Create(OpCodes.Stfld, manager.Fields.Single(f => f.Name == "gameLimit")));
    Console.WriteLine("Patched offline game limit.");
}

static void PatchRouletteStartupActive(ModuleDefinition module)
{
    var method = FindMethod(module, "Commands.RouletteStartCmd", "Reset");
    var instructions = method.Body.Instructions;
    var patched = false;
    for (var i = 0; i < instructions.Count - 2; i++)
    {
        if (instructions[i].Operand is FieldReference field && field.Name == "gameActive"
            && instructions[i + 1].OpCode == OpCodes.Callvirt
            && instructions[i + 1].Operand is MethodReference getter && getter.Name == "get_Value")
            continue;

        if (instructions[i].Operand is FieldReference activeField && activeField.Name == "gameActive")
        {
            for (var j = i + 1; j <= Math.Min(i + 3, instructions.Count - 1); j++)
            {
                if (instructions[j].OpCode == OpCodes.Ldc_I4_0)
                {
                    instructions[j].OpCode = OpCodes.Ldc_I4_1;
                    patched = true;
                    break;
                }
            }
        }
    }
    if (!patched)
        throw new InvalidOperationException("Could not keep RouletteStartCmd.Reset gameActive.");
    Console.WriteLine("Patched roulette startup to remain interactive.");
}

static void PatchOfflineConnection(ModuleDefinition module)
{
    var gameManager = FindType(module, "ViewModel.GameManager");
    foreach (var typeName in new[] { "Commands.TestConnectionCmd", "Commands.TestGameConnectionCmd" })
    {
        var type = FindType(module, typeName);
        var execute = type.Methods.Single(m => m.Name == "Execute");
        var success = type.Methods.Single(m => m.Name == "ConnectionSuccess");
        var managerField = type.Fields.Single(f => f.Name == "_gameManager");
        var onNext = success.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
            .First(m => m.Name == "OnNext");
        var il = execute.Body.GetILProcessor();
        ResetBody(execute);
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldfld, managerField));
        il.Append(il.Create(OpCodes.Ldfld, gameManager.Fields.Single(f => f.Name == "OnApplicationStart")));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(OpCodes.Callvirt, onNext));
        il.Append(il.Create(OpCodes.Ldarg_0));
        il.Append(il.Create(OpCodes.Ldfld, managerField));
        il.Append(il.Create(OpCodes.Ldfld, gameManager.Fields.Single(f => f.Name == "OnConnectionSuccess")));
        il.Append(il.Create(OpCodes.Ldc_I4_1));
        il.Append(il.Create(OpCodes.Callvirt, onNext));
        il.Append(il.Create(OpCodes.Ret));
    }
    Console.WriteLine("Patched startup and START connection checks for offline play.");
}

static void PatchOfflineLogin(ModuleDefinition module)
{
    var type = FindType(module, "Commands.PostUserLoginCmd");
    var execute = type.Methods.Single(m => m.Name == "Execute");
    var managerField = type.Fields.Single(f => f.Name == "_gameManager");
    var gameManager = FindType(module, "ViewModel.GameManager");
    var parse = AllTypes(module)
        .Where(t => t.FullName.StartsWith("Commands.PostUserLoginCmd/", StringComparison.Ordinal))
        .SelectMany(t => t.Methods)
        .Where(m => m.HasBody)
        .SelectMany(m => m.Body.Instructions)
        .Select(i => i.Operand)
        .OfType<MethodReference>()
        .First(m => m.DeclaringType.FullName == "SimpleJSON.JSON" && m.Name == "Parse");
    var onNext = execute.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(m => m.Name == "OnNext");
    var il = execute.Body.GetILProcessor();
    ResetBody(execute);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Stfld, gameManager.Fields.Single(f => f.Name == "userId")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldstr, "offline"));
    il.Append(il.Create(OpCodes.Stfld, gameManager.Fields.Single(f => f.Name == "userAccessToken")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldstr, "{\"user_id\":1,\"accesstoken\":\"offline\",\"referral_link\":\"offline\",\"mobile_number\":\"0810000000\",\"region\":\"KHOMAS\"}"));
    il.Append(il.Create(OpCodes.Call, parse));
    il.Append(il.Create(OpCodes.Stfld, gameManager.Fields.Single(f => f.Name == "UserData")));

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldfld, gameManager.Fields.Single(f => f.Name == "OnLoginSuccess")));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Callvirt, onNext));
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine("Patched login to complete synchronously without a loading wait.");
}

static void PatchLoginButton(ModuleDefinition module)
{
    var type = FindType(module, "Components.LoginButtonInput");
    var method = type.Methods.Single(m => m.Name == "OnClick");
    var gameManager = FindType(module, "ViewModel.GameManager");
    var parse = AllTypes(module)
        .Where(t => t.FullName.StartsWith("Commands.PostUserLoginCmd/", StringComparison.Ordinal))
        .SelectMany(t => t.Methods)
        .Where(m => m.HasBody)
        .SelectMany(m => m.Body.Instructions)
        .Select(i => i.Operand)
        .OfType<MethodReference>()
        .First(m => m.DeclaringType.FullName == "SimpleJSON.JSON" && m.Name == "Parse");
    var getGameScene = gameManager.Methods.Single(m => m.Name == "GetGameScene");
    var gameScene = FindType(module, "ViewModel.GameScene");
    var loadScene = FindType(module, "ViewModel.LoaderManager").Methods
        .Where(m => m.HasBody)
        .SelectMany(m => m.Body.Instructions)
        .Select(i => i.Operand)
        .OfType<MethodReference>()
        .First(m => m.DeclaringType.FullName == "UnityEngine.SceneManagement.SceneManager"
            && m.Name == "LoadScene" && m.Parameters.Count == 1);
    var managerField = type.Fields.Single(f => f.Name == "gameManager");
    var il = method.Body.GetILProcessor();
    ResetBody(method);

    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Stfld, gameManager.Fields.Single(f => f.Name == "userId")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldstr, "offline"));
    il.Append(il.Create(OpCodes.Stfld, gameManager.Fields.Single(f => f.Name == "userAccessToken")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldstr, "{\"user_id\":1,\"accesstoken\":\"offline\",\"referral_link\":\"offline\",\"mobile_number\":\"0810000000\",\"region\":\"KHOMAS\"}"));
    il.Append(il.Create(OpCodes.Call, parse));
    il.Append(il.Create(OpCodes.Stfld, gameManager.Fields.Single(f => f.Name == "UserData")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, managerField));
    il.Append(il.Create(OpCodes.Ldc_I4_3));
    il.Append(il.Create(OpCodes.Callvirt, getGameScene));
    il.Append(il.Create(OpCodes.Ldfld, gameScene.Fields.Single(f => f.Name == "index")));
    il.Append(il.Create(OpCodes.Call, loadScene));
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine("Patched SIGN IN button to load the Unity Game scene directly.");
}

static void PatchOpenMenu(ModuleDefinition module)
{
    var type = FindType(module, "Components.OpenMenuInput");
    var method = type.Methods.Single(m => m.Name == "OnClick");
    var turnState = method.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(m => m.Name == "TurnRouletteState");
    var execute = method.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(m => m.Name == "Execute");
    var setActive = method.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(m => m.Name == "SetActive");
    var il = method.Body.GetILProcessor();
    ResetBody(method);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, type.Fields.Single(f => f.Name == "rouletteCmdFactory")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, type.Fields.Single(f => f.Name == "rouletteManager")));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, type.Fields.Single(f => f.Name == "audioManager")));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Callvirt, turnState));
    il.Append(il.Create(OpCodes.Callvirt, execute));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, type.Fields.Single(f => f.Name == "exitMenu")));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Callvirt, setActive));
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine("Patched hamburger menu to open unconditionally.");
}

static void PatchProbability(ModuleDefinition module)
{
    var method = FindMethod(module, "APIManager", "CallProbabilityAPI");
    var probabilityData = FindType(module, "ProbabilityData");
    var probabilityValues = FindType(module, "ProbabilityAutoJackpot");
    var singleton = FindType(module, "Singleton");
    var dataManager = FindType(module, "DataManager");
    var il = method.Body.GetILProcessor();
    ResetBody(method);
    method.Body.InitLocals = true;
    var dataLocal = new VariableDefinition(probabilityData);
    var valuesLocal = new VariableDefinition(probabilityValues);
    method.Body.Variables.Add(dataLocal);
    method.Body.Variables.Add(valuesLocal);

    il.Append(il.Create(OpCodes.Newobj, probabilityData.Methods.Single(m => m.IsConstructor && !m.IsStatic && !m.HasParameters)));
    il.Append(il.Create(OpCodes.Stloc, dataLocal));
    il.Append(il.Create(OpCodes.Newobj, probabilityValues.Methods.Single(m => m.IsConstructor && !m.IsStatic && !m.HasParameters)));
    il.Append(il.Create(OpCodes.Stloc, valuesLocal));
    il.Append(il.Create(OpCodes.Ldloc, valuesLocal));
    il.Append(il.Create(OpCodes.Ldstr, "OFFLINE"));
    il.Append(il.Create(OpCodes.Stfld, probabilityValues.Fields.Single(f => f.Name == "label")));
    il.Append(il.Create(OpCodes.Ldloc, valuesLocal));
    il.Append(il.Create(OpCodes.Ldc_I4_S, (sbyte)10));
    il.Append(il.Create(OpCodes.Stfld, probabilityValues.Fields.Single(f => f.Name == "rwm")));
    il.Append(il.Create(OpCodes.Ldloc, valuesLocal));
    il.Append(il.Create(OpCodes.Ldc_I4, 1_000_000));
    il.Append(il.Create(OpCodes.Stfld, probabilityValues.Fields.Single(f => f.Name == "autoJackpot")));
    il.Append(il.Create(OpCodes.Ldloc, dataLocal));
    il.Append(il.Create(OpCodes.Ldloc, valuesLocal));
    il.Append(il.Create(OpCodes.Stfld, probabilityData.Fields.Single(f => f.Name == "data")));
    il.Append(il.Create(OpCodes.Call, singleton.Methods.Single(m => m.Name == "get_Instance")));
    il.Append(il.Create(OpCodes.Ldfld, singleton.Fields.Single(f => f.Name == "dataManager")));
    il.Append(il.Create(OpCodes.Ldloc, dataLocal));
    il.Append(il.Create(OpCodes.Stfld, dataManager.Fields.Single(f => f.Name == "probabilityData")));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Ldstr, ""));
    il.Append(il.Create(OpCodes.Callvirt, module.ImportReference(typeof(Action<bool, string>).GetMethod("Invoke")!)));
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine("Patched local probability response.");
}

static void PatchNoOp(ModuleDefinition module, string typeName, string methodName)
{
    var method = FindMethod(module, typeName, methodName);
    var il = method.Body.GetILProcessor();
    ResetBody(method);
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine($"Disabled network call {typeName}.{methodName}.");
}

static void PatchJsonApi(ModuleDefinition module, string methodName, string iteratorTypeName, string dataManagerField, string json)
{
    var method = FindMethod(module, "APIManager", methodName);
    var iterator = FindType(module, iteratorTypeName).Methods.Single(m => m.Name == "MoveNext");
    var fromJson = iterator.Body.Instructions.Select(i => i.Operand).OfType<MethodReference>()
        .First(m => m.Name == "FromJson");
    var singleton = FindType(module, "Singleton");
    var dataManager = FindType(module, "DataManager");
    var targetField = dataManager.Fields.Single(f => f.Name == dataManagerField);
    var il = method.Body.GetILProcessor();
    ResetBody(method);
    il.Append(il.Create(OpCodes.Call, singleton.Methods.Single(m => m.Name == "get_Instance")));
    il.Append(il.Create(OpCodes.Ldfld, singleton.Fields.Single(f => f.Name == "dataManager")));
    il.Append(il.Create(OpCodes.Ldstr, json));
    il.Append(il.Create(OpCodes.Call, fromJson));
    il.Append(il.Create(OpCodes.Stfld, targetField));
    il.Append(il.Create(OpCodes.Ldarg_1));
    il.Append(il.Create(OpCodes.Ldc_I4_1));
    il.Append(il.Create(OpCodes.Ldstr, ""));
    il.Append(il.Create(OpCodes.Callvirt, module.ImportReference(typeof(Action<bool, string>).GetMethod("Invoke")!)));
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine($"Patched local JSON response for APIManager.{methodName}.");
}

static void PatchStringIterator(ModuleDefinition module, string typeName, string response)
{
    var type = FindType(module, typeName);
    var method = type.Methods.Single(m => m.Name == "MoveNext");
    var observer = type.Fields.Single(f => f.Name == "observer");
    var onNext = module.ImportReference(typeof(IObserver<string>).GetMethod("OnNext")!);
    var onCompleted = module.ImportReference(typeof(IObserver<string>).GetMethod("OnCompleted")!);
    var il = method.Body.GetILProcessor();
    ResetBody(method);
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, observer));
    il.Append(il.Create(OpCodes.Ldstr, response));
    il.Append(il.Create(OpCodes.Callvirt, onNext));
    il.Append(il.Create(OpCodes.Ldarg_0));
    il.Append(il.Create(OpCodes.Ldfld, observer));
    il.Append(il.Create(OpCodes.Callvirt, onCompleted));
    il.Append(il.Create(OpCodes.Ldc_I4_0));
    il.Append(il.Create(OpCodes.Ret));
    Console.WriteLine($"Patched offline response for {typeName}.");
}

static void InjectApk(string inputApk, string outputApk, string scriptsDll)
{
    inputApk = Path.GetFullPath(inputApk);
    outputApk = Path.GetFullPath(outputApk);
    scriptsDll = Path.GetFullPath(scriptsDll);
    Directory.CreateDirectory(Path.GetDirectoryName(outputApk)!);
    File.Copy(inputApk, outputApk, overwrite: true);

    using var archive = ZipFile.Open(outputApk, ZipArchiveMode.Update);
    var managedPath = "assets/bin/Data/Managed/Scripts.dll";
    archive.GetEntry(managedPath)?.Delete();
    foreach (var signature in archive.Entries.Where(e =>
        e.FullName.Equals("META-INF/MANIFEST.MF", StringComparison.OrdinalIgnoreCase)
        || e.FullName.StartsWith("META-INF/", StringComparison.OrdinalIgnoreCase)
            && (e.FullName.EndsWith(".RSA", StringComparison.OrdinalIgnoreCase)
                || e.FullName.EndsWith(".DSA", StringComparison.OrdinalIgnoreCase)
                || e.FullName.EndsWith(".EC", StringComparison.OrdinalIgnoreCase)
                || e.FullName.EndsWith(".SF", StringComparison.OrdinalIgnoreCase))).ToArray())
    {
        signature.Delete();
    }

    var entry = archive.CreateEntry(managedPath, CompressionLevel.Optimal);
    entry.LastWriteTime = DateTimeOffset.UtcNow;
    using var destination = entry.Open();
    using var source = File.OpenRead(scriptsDll);
    source.CopyTo(destination);
    Console.WriteLine($"Injected patched Scripts.dll into {outputApk}");
}
