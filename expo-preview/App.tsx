import { StatusBar } from 'expo-status-bar';
import React, { useRef, useState } from 'react';
import {
  Animated,
  Image,
  ImageBackground,
  KeyboardAvoidingView,
  Modal,
  Platform,
  Pressable,
  SafeAreaView,
  ScrollView,
  StyleSheet,
  Text,
  TextInput,
  View,
} from 'react-native';

const art = {
  logo: require('./assets/latest/kasikili-logo.png'),
  title: require('./assets/latest/kasikili-title.png'),
  greenBox: require('./assets/latest/box_green.png'),
  orangeBox: require('./assets/latest/box_orange.png'),
  orangeSmall: require('./assets/latest/box_orange_sm.png'),
  violetBox: require('./assets/latest/box-violet-2.png'),
  whiteBox: require('./assets/latest/box-white.png'),
  line: require('./assets/latest/line_0.png'),
  signup: require('./assets/latest/sign-up-here.png'),
  forgot: require('./assets/latest/forget-password.png'),
  terms: require('./assets/latest/terms-blue.png'),
  wheel: require('./assets/latest/wheel.png'),
  wheelCredit: require('./assets/latest/wheel_credit.png'),
  wheelWin: require('./assets/latest/wheel_win.png'),
  greenCell: require('./assets/latest/circle_green.png'),
  redCell: require('./assets/latest/circle_red.png'),
  blackCell: require('./assets/latest/circle_black.png'),
  bet: require('./assets/latest/button_bet.png'),
  start: require('./assets/latest/button_start.png'),
  cancel: require('./assets/latest/button_cancel.png'),
  menu: require('./assets/latest/button_menu.png'),
  probability: require('./assets/latest/spin_probability.png'),
  gameLines: require('./assets/latest/lines.png'),
  menuBackdrop: require('./assets/latest/ui_menu.png'),
  crown: require('./assets/latest/Top 3.png'),
  leaderRow: require('./assets/latest/bg (3).png'),
  playerRow: require('./assets/latest/bg (2).png'),
  arrowUp: require('./assets/latest/Polygon 8.png'),
  arrowDown: require('./assets/latest/Polygon 6.png'),
  refresh: require('./assets/latest/Group 2600.png'),
  download: require('./assets/latest/Group 2601.png'),
  close: require('./assets/latest/sign-out-square.png'),
  photo: require('./assets/latest/Photo.png'),
  accountTitle: require('./assets/latest/account-title.png'),
  mobileLabel: require('./assets/latest/Mobile Number.png'),
  regionLabel: require('./assets/latest/Region.png'),
  balanceLabel: require('./assets/latest/balance-label.png'),
  accountInput: require('./assets/latest/Rectangle 18.png'),
  cashoutLabel: require('./assets/latest/cashout-label.png'),
  bitcoinWallet: require('./assets/latest/bitcoin-wallet.png'),
  referral: require('./assets/latest/Untitled-1.png'),
  historyTitle: require('./assets/latest/history-title.png'),
  tableTitles: require('./assets/latest/table-titles.png'),
};

type Screen = 'login' | 'register' | 'game' | 'leaders' | 'wallet';

const numbers = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
const redNumbers = new Set([1, 3, 5, 7, 9, 11]);
const leaders = [
  ['1', '26481 XXX 2569', '98%', 'up'],
  ['2', '26481 XXX 1074', '91%', 'up'],
  ['3', '26485 XXX 6812', '86%', 'down'],
  ['4', '26481 XXX 4430', '78%', 'up'],
  ['5', '26481 XXX 9032', '72%', 'down'],
  ['6', '26485 XXX 2215', '69%', 'up'],
  ['7', '26481 XXX 7724', '63%', 'down'],
  ['8', '26485 XXX 1408', '58%', 'up'],
  ['9', '26481 XXX 6120', '54%', 'down'],
  ['10', '26481 XXX 3051', '49%', 'up'],
] as const;

function ArtButton({
  source,
  label,
  onPress,
  small,
}: {
  source: number;
  label: string;
  onPress: () => void;
  small?: boolean;
}) {
  return (
    <Pressable onPress={onPress} style={({ pressed }) => [styles.artButton, small && styles.artButtonSmall, pressed && styles.pressed]}>
      <ImageBackground source={source} resizeMode="stretch" style={styles.artButtonBackground}>
        <Text style={[styles.artButtonLabel, small && styles.artButtonLabelSmall]}>{label}</Text>
      </ImageBackground>
    </Pressable>
  );
}

function LoginScreen({ onLogin, onRegister }: { onLogin: () => void; onRegister: () => void }) {
  const [remember, setRemember] = useState(true);
  return (
    <KeyboardAvoidingView style={styles.loginRoot} behavior={Platform.OS === 'ios' ? 'padding' : undefined}>
      <StatusBar style="dark" />
      <ScrollView contentContainerStyle={styles.loginScroll} keyboardShouldPersistTaps="handled">
        <Image source={art.title} resizeMode="contain" style={styles.loginTitle} />
        <Image source={art.logo} resizeMode="contain" style={styles.loginLogo} />
        <Image source={art.line} resizeMode="stretch" style={styles.divider} />

        <ImageBackground source={art.greenBox} resizeMode="stretch" style={styles.loginField}>
          <Text style={styles.fieldLabel}>MOBILE NUMBER</Text>
          <TextInput
            accessibilityLabel="Mobile number"
            keyboardType="phone-pad"
            placeholder="264 81 000 0000"
            placeholderTextColor="#bdd5c4"
            style={styles.fieldInput}
          />
        </ImageBackground>
        <ImageBackground source={art.greenBox} resizeMode="stretch" style={styles.loginField}>
          <Text style={styles.fieldLabel}>PASSWORD</Text>
          <TextInput
            accessibilityLabel="Password"
            secureTextEntry
            placeholder="••••••••"
            placeholderTextColor="#bdd5c4"
            style={styles.fieldInput}
          />
        </ImageBackground>

        <View style={styles.rememberRow}>
          <Text style={styles.rememberLabel}>REMEMBER PASSWORD ?</Text>
          <Pressable onPress={() => setRemember(false)} style={[styles.choice, !remember && styles.choiceActive]}>
            <Text style={[styles.choiceText, !remember && styles.choiceTextActive]}>NO</Text>
          </Pressable>
          <Text style={styles.choicePipe}>|</Text>
          <Pressable onPress={() => setRemember(true)} style={[styles.choice, remember && styles.choiceActive]}>
            <Text style={[styles.choiceText, remember && styles.choiceTextActive]}>YES</Text>
          </Pressable>
        </View>

        <ArtButton source={art.orangeBox} label="LOGIN" onPress={onLogin} />
        <View style={styles.loginLinks}>
          <Pressable onPress={onRegister}><Image source={art.signup} resizeMode="contain" style={styles.signupLink} /></Pressable>
          <Image source={art.forgot} resizeMode="contain" style={styles.forgotLink} />
        </View>
        <Image source={art.terms} resizeMode="contain" style={styles.terms} />
        <Text style={styles.version}>VERSION 1.6.0  |  © Copy of Kasikili Virtual Gaming cc  |  2025</Text>
      </ScrollView>
    </KeyboardAvoidingView>
  );
}

function RegisterScreen({ onBack }: { onBack: () => void }) {
  return (
    <SafeAreaView style={styles.loginRoot}>
      <StatusBar style="dark" />
      <ScrollView contentContainerStyle={styles.registerScroll} keyboardShouldPersistTaps="handled">
        <Image source={art.title} resizeMode="contain" style={styles.loginTitle} />
        <Image source={art.logo} resizeMode="contain" style={styles.registerLogo} />
        <Text style={styles.formHeading}>CREATE AN ACCOUNT</Text>
        {['MOBILE NUMBER', 'PASSWORD', 'CONFIRM PASSWORD', 'REFERRAL CODE (OPTIONAL)'].map((label) => (
          <ImageBackground key={label} source={art.greenBox} resizeMode="stretch" style={styles.registerField}>
            <Text style={styles.fieldLabel}>{label}</Text>
            <TextInput secureTextEntry={label.includes('PASSWORD')} style={styles.fieldInput} placeholderTextColor="#bdd5c4" />
          </ImageBackground>
        ))}
        <ArtButton source={art.orangeBox} label="SIGN UP" onPress={onBack} />
        <Pressable onPress={onBack}><Text style={styles.backToLogin}>ALREADY REGISTERED?  LOGIN HERE</Text></Pressable>
      </ScrollView>
    </SafeAreaView>
  );
}

function NumberCell({ number, selected, onPress }: { number: number; selected: boolean; onPress: () => void }) {
  const source = number === 0 ? art.greenCell : redNumbers.has(number) ? art.redCell : art.blackCell;
  return (
    <Pressable onPress={onPress} style={[styles.numberCell, selected && styles.numberSelected]}>
      <ImageBackground source={source} resizeMode="contain" style={styles.numberCellBackground}>
        <Text style={styles.numberText}>{number}</Text>
        <Image source={art.bet} resizeMode="contain" style={styles.betChip} />
      </ImageBackground>
    </Pressable>
  );
}

function GameScreen({
  onLeaders,
  onWallet,
  onLogout,
}: {
  onLeaders: () => void;
  onWallet: () => void;
  onLogout: () => void;
}) {
  const [menuOpen, setMenuOpen] = useState(false);
  const [selected, setSelected] = useState<number | null>(7);
  const [credits, setCredits] = useState(100);
  const [win, setWin] = useState(0);
  const [spinning, setSpinning] = useState(false);
  const spin = useRef(new Animated.Value(0)).current;
  const rotation = spin.interpolate({ inputRange: [0, 1], outputRange: ['0deg', '1800deg'] });

  const play = () => {
    if (spinning || selected === null || credits < 5) return;
    setSpinning(true);
    setCredits((value) => value - 5);
    setWin(0);
    spin.setValue(0);
    Animated.timing(spin, { toValue: 1, duration: 2300, useNativeDriver: true }).start(() => {
      const winner = numbers[Math.floor(Math.random() * numbers.length)];
      if (winner === selected) {
        setCredits((value) => value + 50);
        setWin(50);
      }
      setSpinning(false);
    });
  };

  return (
    <SafeAreaView style={styles.gameRoot}>
      <StatusBar style="light" />
      <View style={styles.gameHeader}>
        <Pressable onPress={() => setMenuOpen(true)} style={styles.menuButton}>
          <Image source={art.menu} resizeMode="contain" style={styles.fill} />
        </Pressable>
        <Text style={styles.gameTitle}>KASIKILI BERGMANN ROULETTE</Text>
        <View style={styles.menuButton} />
      </View>

      <ScrollView contentContainerStyle={styles.gameScroll} showsVerticalScrollIndicator={false}>
        <View style={styles.wheelZone}>
          <Image source={art.gameLines} resizeMode="contain" style={styles.gameLines} />
          <Animated.Image source={art.wheel} resizeMode="contain" style={[styles.wheel, { transform: [{ rotate: rotation }] }]} />
        </View>
        <View style={styles.scoreRow}>
          <ImageBackground source={art.wheelCredit} resizeMode="contain" style={styles.scoreArt}>
            <Text style={styles.scoreNumber}>{credits}</Text>
          </ImageBackground>
          <ImageBackground source={art.wheelWin} resizeMode="contain" style={styles.scoreArt}>
            <Text style={styles.scoreNumber}>{win}</Text>
          </ImageBackground>
        </View>
        <Image source={art.probability} resizeMode="contain" style={styles.probability} />
        <Text style={styles.pickLabel}>SELECT A NUMBER</Text>
        <View style={styles.numberGrid}>
          {numbers.map((number) => (
            <NumberCell key={number} number={number} selected={selected === number} onPress={() => setSelected(number)} />
          ))}
        </View>
        <Text style={styles.stakeText}>BET: N$ 5.00</Text>
        <View style={styles.playRow}>
          <Pressable onPress={() => setSelected(null)} style={({ pressed }) => [styles.gameAction, pressed && styles.pressed]}>
            <Image source={art.cancel} resizeMode="contain" style={styles.fill} />
          </Pressable>
          <Pressable onPress={play} style={({ pressed }) => [styles.gameAction, pressed && styles.pressed, spinning && styles.disabled]}>
            <Image source={art.start} resizeMode="contain" style={styles.fill} />
          </Pressable>
        </View>
      </ScrollView>

      <Modal transparent visible={menuOpen} animationType="fade" onRequestClose={() => setMenuOpen(false)}>
        <Pressable style={styles.modalShade} onPress={() => setMenuOpen(false)}>
          <ImageBackground source={art.menuBackdrop} resizeMode="stretch" style={styles.gameMenu}>
            <Text style={styles.menuHeading}>MENU</Text>
            <ArtButton source={art.orangeSmall} label="CASH OUT" onPress={() => { setMenuOpen(false); onWallet(); }} small />
            <ArtButton source={art.violetBox} label="BUY CREDITS" onPress={() => setMenuOpen(false)} small />
            <ArtButton source={art.orangeSmall} label="LEADERBOARD" onPress={() => { setMenuOpen(false); onLeaders(); }} small />
            <ArtButton source={art.whiteBox} label="SIGN OUT" onPress={onLogout} small />
            <Pressable onPress={() => setMenuOpen(false)}><Text style={styles.closeMenu}>CLOSE</Text></Pressable>
          </ImageBackground>
        </Pressable>
      </Modal>
    </SafeAreaView>
  );
}

function LeaderboardScreen({ onBack }: { onBack: () => void }) {
  return (
    <SafeAreaView style={styles.greenRoot}>
      <StatusBar style="light" />
      <ScrollView contentContainerStyle={styles.leaderScroll}>
        <View style={styles.pageHeader}>
          <View style={styles.closeIcon} />
          <Text style={styles.pageTitle}>Leaderboard</Text>
          <Pressable onPress={onBack}><Image source={art.close} style={styles.closeIcon} resizeMode="contain" /></Pressable>
        </View>
        <View style={styles.crownBlock}>
          <Image source={art.crown} resizeMode="contain" style={styles.crown} />
          <Text style={styles.crownScore}>5000</Text>
          <Text style={styles.crownCaption}>LEADERBOARD PRIZE</Text>
        </View>
        <View style={styles.leaderToolbar}>
          <View><Text style={styles.sectionTitle}>Active Referrals</Text><Text style={styles.sectionSub}>Top 10 players this month</Text></View>
          <Pressable><Image source={art.refresh} resizeMode="contain" style={styles.toolIcon} /></Pressable>
          <Pressable><Image source={art.download} resizeMode="contain" style={styles.toolIcon} /></Pressable>
        </View>
        <View style={styles.tableHeader}><Text style={styles.rankHead}>#</Text><Text style={styles.mobileHead}>MOBILE NUMBER</Text><Text style={styles.scoreHead}>SCORE</Text></View>
        {leaders.map(([rank, mobile, score, direction]) => (
          <ImageBackground key={rank} source={art.leaderRow} resizeMode="stretch" style={styles.leaderRow}>
            <Text style={styles.rank}>{rank}</Text>
            <Text style={styles.maskedMobile}>{mobile}</Text>
            <Text style={styles.percent}>{score}</Text>
            <Image source={direction === 'up' ? art.arrowUp : art.arrowDown} resizeMode="contain" style={styles.arrow} />
          </ImageBackground>
        ))}
        <Text style={styles.yourPosition}>YOUR POSITION</Text>
        <ImageBackground source={art.playerRow} resizeMode="stretch" style={styles.playerPosition}>
          <Text style={styles.rank}>24</Text>
          <Image source={art.photo} style={styles.playerPhoto} />
          <Text style={styles.maskedMobile}>26481 XXX 2026</Text>
          <Text style={styles.percent}>31%</Text>
        </ImageBackground>
        <View style={styles.prizeInfo}>
          <Text style={styles.prizeTitle}>PRIZE BREAKDOWN</Text>
          <Text style={styles.prizeText}>1st 2000  •  2nd 1000  •  3rd 600</Text>
          <Text style={styles.prizeText}>4th – 10th 200 each</Text>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}

function WalletScreen({ onBack }: { onBack: () => void }) {
  return (
    <SafeAreaView style={styles.walletRoot}>
      <StatusBar style="dark" />
      <ScrollView contentContainerStyle={styles.walletScroll}>
        <View style={styles.walletHeader}>
          <Image source={art.accountTitle} resizeMode="contain" style={styles.accountTitle} />
          <Pressable onPress={onBack}><Image source={art.close} style={styles.walletClose} resizeMode="contain" /></Pressable>
        </View>
        <Image source={art.mobileLabel} resizeMode="contain" style={styles.walletLabel} />
        <ImageBackground source={art.accountInput} resizeMode="stretch" style={styles.walletField}><Text style={styles.walletValue}>264 81 000 2026</Text></ImageBackground>
        <Image source={art.regionLabel} resizeMode="contain" style={styles.walletLabel} />
        <ImageBackground source={art.accountInput} resizeMode="stretch" style={styles.walletField}><Text style={styles.walletValue}>NAMIBIA</Text></ImageBackground>
        <Image source={art.balanceLabel} resizeMode="contain" style={styles.balanceLabel} />
        <Text style={styles.balance}>N$ 100.00</Text>
        <Image source={art.cashoutLabel} resizeMode="contain" style={styles.cashoutLabel} />
        <Image source={art.bitcoinWallet} resizeMode="contain" style={styles.bitcoin} />
        <ArtButton source={art.greenBox} label="REQUEST CASH OUT" onPress={() => undefined} />
        <Image source={art.referral} resizeMode="contain" style={styles.referral} />
        <Image source={art.historyTitle} resizeMode="contain" style={styles.historyTitle} />
        <Image source={art.tableTitles} resizeMode="contain" style={styles.tableTitles} />
        <Text style={styles.emptyHistory}>No transactions yet</Text>
      </ScrollView>
    </SafeAreaView>
  );
}

export default function App() {
  const [screen, setScreen] = useState<Screen>('login');
  if (screen === 'login') return <LoginScreen onLogin={() => setScreen('game')} onRegister={() => setScreen('register')} />;
  if (screen === 'register') return <RegisterScreen onBack={() => setScreen('login')} />;
  if (screen === 'leaders') return <LeaderboardScreen onBack={() => setScreen('game')} />;
  if (screen === 'wallet') return <WalletScreen onBack={() => setScreen('game')} />;
  return <GameScreen onLeaders={() => setScreen('leaders')} onWallet={() => setScreen('wallet')} onLogout={() => setScreen('login')} />;
}

const styles = StyleSheet.create({
  fill: { width: '100%', height: '100%' },
  pressed: { opacity: 0.72, transform: [{ scale: 0.985 }] },
  disabled: { opacity: 0.55 },
  loginRoot: { flex: 1, backgroundColor: '#ffffff' },
  loginScroll: { alignItems: 'center', paddingHorizontal: 24, paddingTop: 25, paddingBottom: 26 },
  loginTitle: { width: '92%', height: 34, marginBottom: 10 },
  loginLogo: { width: 278, height: 282, marginTop: -6, marginBottom: -8 },
  registerLogo: { width: 180, height: 182, marginVertical: 8 },
  divider: { width: '96%', height: 3, marginBottom: 18 },
  loginField: { width: '100%', height: 68, justifyContent: 'center', paddingHorizontal: 23, marginBottom: 12 },
  registerField: { width: '100%', height: 63, justifyContent: 'center', paddingHorizontal: 22, marginBottom: 9 },
  fieldLabel: { color: '#ffffff', fontSize: 10, fontWeight: '800', letterSpacing: 1.25, marginBottom: 1 },
  fieldInput: { color: '#ffffff', fontSize: 17, fontWeight: '600', paddingVertical: 3 },
  rememberRow: { flexDirection: 'row', alignItems: 'center', alignSelf: 'stretch', justifyContent: 'center', marginVertical: 4 },
  rememberLabel: { color: '#526274', fontSize: 10, fontWeight: '800', letterSpacing: 0.6, marginRight: 8 },
  choice: { paddingHorizontal: 7, paddingVertical: 5, borderRadius: 10 },
  choiceActive: { backgroundColor: '#157649' },
  choiceText: { color: '#708090', fontSize: 11, fontWeight: '900' },
  choiceTextActive: { color: '#fff' },
  choicePipe: { color: '#bcc5cd' },
  artButton: { width: '100%', height: 64, alignItems: 'center', justifyContent: 'center', marginTop: 10, overflow: 'hidden', borderRadius: 30 },
  artButtonBackground: { width: '100%', height: '100%', alignItems: 'center', justifyContent: 'center' },
  artButtonSmall: { width: 245, height: 52, marginTop: 10 },
  artButtonLabel: { color: '#fff', fontSize: 17, fontWeight: '900', letterSpacing: 1.4, textShadowColor: 'rgba(0,0,0,.28)', textShadowRadius: 2, textShadowOffset: { width: 0, height: 1 } },
  artButtonLabelSmall: { fontSize: 14 },
  loginLinks: { width: '100%', flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginTop: 16 },
  signupLink: { width: 118, height: 23 },
  forgotLink: { width: 122, height: 23 },
  terms: { width: 148, height: 27, marginTop: 18 },
  version: { marginTop: 8, color: '#75808a', fontSize: 7.5, textAlign: 'center' },
  registerScroll: { alignItems: 'center', paddingHorizontal: 24, paddingTop: 24, paddingBottom: 30 },
  formHeading: { color: '#17633f', fontSize: 18, fontWeight: '900', letterSpacing: 1.5, marginBottom: 14 },
  backToLogin: { color: '#215f8d', fontSize: 11, fontWeight: '900', letterSpacing: 0.6, marginTop: 18 },
  gameRoot: { flex: 1, backgroundColor: '#314d79' },
  gameHeader: { height: 62, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingHorizontal: 8 },
  menuButton: { width: 48, height: 48 },
  gameTitle: { flex: 1, color: '#fff', fontSize: 14, fontWeight: '900', letterSpacing: 0.6, textAlign: 'center', textShadowColor: '#17263d', textShadowRadius: 2 },
  gameScroll: { alignItems: 'center', paddingBottom: 24 },
  wheelZone: { width: 250, height: 250, alignItems: 'center', justifyContent: 'center' },
  wheel: { width: 226, height: 226 },
  gameLines: { position: 'absolute', width: 278, height: 278, opacity: 0.42 },
  scoreRow: { flexDirection: 'row', marginTop: -3, gap: 14 },
  scoreArt: { width: 126, height: 52, justifyContent: 'flex-end', alignItems: 'center', paddingBottom: 4 },
  scoreNumber: { color: '#fff', fontSize: 18, fontWeight: '900' },
  probability: { width: 300, height: 45, marginTop: 5 },
  pickLabel: { color: '#fff', fontSize: 12, fontWeight: '900', letterSpacing: 1.5, marginVertical: 2 },
  numberGrid: { width: 268, flexDirection: 'row', flexWrap: 'wrap', justifyContent: 'center', gap: 4 },
  numberCell: { width: 61, height: 61, alignItems: 'center', justifyContent: 'center' },
  numberCellBackground: { width: '100%', height: '100%', alignItems: 'center', justifyContent: 'center' },
  numberSelected: { borderWidth: 3, borderColor: '#ffd94d', borderRadius: 31, transform: [{ scale: 1.05 }] },
  numberText: { color: '#fff', fontSize: 19, fontWeight: '900', textShadowColor: '#000', textShadowRadius: 2 },
  betChip: { position: 'absolute', right: -1, bottom: -2, width: 22, height: 22 },
  stakeText: { color: '#fff', fontSize: 12, fontWeight: '900', marginTop: 9, letterSpacing: 1 },
  playRow: { flexDirection: 'row', gap: 18, marginTop: 3 },
  gameAction: { width: 137, height: 70 },
  modalShade: { flex: 1, backgroundColor: 'rgba(5,15,20,.7)', alignItems: 'center', justifyContent: 'center' },
  gameMenu: { width: 315, height: 430, alignItems: 'center', justifyContent: 'center', padding: 25 },
  menuHeading: { color: '#fff', fontSize: 26, fontWeight: '900', letterSpacing: 3, marginBottom: 8 },
  closeMenu: { color: '#fff', fontSize: 11, fontWeight: '900', letterSpacing: 2, marginTop: 18 },
  greenRoot: { flex: 1, backgroundColor: '#0a5738' },
  leaderScroll: { padding: 18, paddingBottom: 35 },
  pageHeader: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  pageTitle: { color: '#fff', fontSize: 27, fontWeight: '800' },
  closeIcon: { width: 34, height: 34 },
  crownBlock: { alignItems: 'center', marginTop: 5 },
  crown: { width: 120, height: 156 },
  crownScore: { position: 'absolute', top: 82, color: '#fff', fontSize: 23, fontWeight: '900' },
  crownCaption: { color: '#f5cf4a', fontSize: 10, fontWeight: '900', letterSpacing: 1.2, marginTop: -7 },
  leaderToolbar: { flexDirection: 'row', alignItems: 'center', gap: 9, marginVertical: 15 },
  sectionTitle: { color: '#fff', fontSize: 19, fontWeight: '900' },
  sectionSub: { color: '#a9d4be', fontSize: 10, marginTop: 2 },
  toolIcon: { width: 34, height: 34 },
  tableHeader: { flexDirection: 'row', paddingHorizontal: 15, marginBottom: 5 },
  rankHead: { width: 30, color: '#9fd4b7', fontSize: 9, fontWeight: '900' },
  mobileHead: { flex: 1, color: '#9fd4b7', fontSize: 9, fontWeight: '900' },
  scoreHead: { width: 50, color: '#9fd4b7', fontSize: 9, fontWeight: '900' },
  leaderRow: { height: 51, flexDirection: 'row', alignItems: 'center', paddingHorizontal: 15, marginBottom: 4 },
  rank: { width: 30, color: '#fff', fontSize: 14, fontWeight: '900' },
  maskedMobile: { flex: 1, color: '#fff', fontSize: 13, fontWeight: '700' },
  percent: { width: 42, color: '#fff', fontSize: 14, fontWeight: '900', textAlign: 'right' },
  arrow: { width: 13, height: 13, marginLeft: 8 },
  yourPosition: { color: '#f6d14b', fontSize: 10, fontWeight: '900', letterSpacing: 1.4, marginTop: 12, marginBottom: 5 },
  playerPosition: { height: 56, flexDirection: 'row', alignItems: 'center', paddingHorizontal: 15 },
  playerPhoto: { width: 30, height: 30, borderRadius: 15, marginRight: 8 },
  prizeInfo: { borderWidth: 1, borderColor: '#4f9c79', padding: 14, marginTop: 18, borderRadius: 10, alignItems: 'center' },
  prizeTitle: { color: '#f4cf4b', fontSize: 12, fontWeight: '900', letterSpacing: 1.5 },
  prizeText: { color: '#fff', fontSize: 11, marginTop: 5 },
  walletRoot: { flex: 1, backgroundColor: '#f6f6f3' },
  walletScroll: { padding: 23, paddingBottom: 40, alignItems: 'center' },
  walletHeader: { width: '100%', flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', marginBottom: 14 },
  accountTitle: { width: 190, height: 52 },
  walletClose: { width: 36, height: 36 },
  walletLabel: { width: '100%', height: 23, alignSelf: 'flex-start', marginTop: 8 },
  walletField: { width: '100%', height: 62, justifyContent: 'center', paddingHorizontal: 21 },
  walletValue: { color: '#26513e', fontSize: 15, fontWeight: '800' },
  balanceLabel: { width: 120, height: 30, marginTop: 18 },
  balance: { color: '#0a653f', fontSize: 32, fontWeight: '900', marginTop: -2 },
  cashoutLabel: { width: 170, height: 38, marginTop: 18 },
  bitcoin: { width: 124, height: 92, marginVertical: 5 },
  referral: { width: 210, height: 56, marginTop: 17 },
  historyTitle: { width: 175, height: 45, marginTop: 18 },
  tableTitles: { width: '100%', height: 40, marginTop: 6 },
  emptyHistory: { color: '#809087', fontSize: 12, marginTop: 16 },
});
