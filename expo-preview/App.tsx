import { StatusBar } from 'expo-status-bar';
import React, { useRef, useState } from 'react';
import {
  Animated,
  Image,
  Modal,
  Pressable,
  SafeAreaView,
  ScrollView,
  StyleSheet,
  Switch,
  Text,
  TextInput,
  View,
  useWindowDimensions,
} from 'react-native';

const art = {
  wheel: require('./assets/latest/wheel.png'),
  win: require('./assets/latest/wheel_win.png'),
  credit: require('./assets/latest/wheel_credit.png'),
  crown: require('./assets/latest/Top 3.png'),
};

type Screen = 'login' | 'signup' | 'forgot' | 'game' | 'account' | 'notifications' | 'leaderboard';

const multipliers = ['10', '50', '1000', '2000', '25', '100'];
const numberRows = [[1, 2, 3], [4, 5, 6], [7, 8, 9], [10, 11, 12]];
const redNumbers = new Set([1, 3, 5, 7, 9, 11]);
const leaders = [
  ['1', '26481 XXX 2569', '97%', 'up', 'gold'],
  ['2', '26481 XXX 2569', '92%', 'down', 'white'],
  ['3', '26481 XXX 2569', '88%', 'down', 'white'],
  ['4', '26481 XXX 2569', '76%', 'up', 'dark'],
  ['5', '26481 XXX 2569', '75%', 'flat', 'dark'],
  ['6', '26481 XXX 2569', '69%', 'up', 'dark'],
  ['7', '26481 XXX 2569', '35%', 'flat', 'dark'],
  ['8', '26481 XXX 2569', '30%', 'down', 'dark'],
  ['9', '26481 XXX 2569', '26%', 'down', 'dark'],
  ['10', '26481 XXX 2569', '18%', 'down', 'dark'],
] as const;

function AppStatusBar() {
  return <StatusBar hidden={false} style="light" backgroundColor="#0f393b" translucent={false} />;
}

function BackIcon({ onPress, light = false }: { onPress: () => void; light?: boolean }) {
  return (
    <Pressable accessibilityRole="button" accessibilityLabel="Back" onPress={onPress} style={styles.backButton}>
      <View style={[styles.backDoor, light && styles.backDoorLight]} />
      <Text style={[styles.backArrow, light && styles.backArrowLight]}>←</Text>
    </Pressable>
  );
}

function Hamburger({ onPress }: { onPress: () => void }) {
  return (
    <Pressable accessibilityRole="button" accessibilityLabel="Menu" onPress={onPress} style={styles.hamburger}>
      <View style={styles.hamburgerLine} />
      <View style={styles.hamburgerLine} />
      <View style={styles.hamburgerLine} />
    </Pressable>
  );
}

function AuthHeader({ compact = false }: { compact?: boolean }) {
  return (
    <>
      <Text style={styles.authTitle}>KASIKILI BERGMANN ROULETTE</Text>
      <View style={styles.authRule} />
      <Image source={art.wheel} resizeMode="contain" style={compact ? styles.authWheelCompact : styles.authWheel} />
    </>
  );
}

function AuthField({ label, secure = false }: { label: string; secure?: boolean }) {
  return (
    <TextInput
      accessibilityLabel={label}
      placeholder={label}
      placeholderTextColor="#ffffff"
      secureTextEntry={secure}
      autoCapitalize="none"
      style={styles.authField}
    />
  );
}

function OrangeButton({ label, onPress }: { label: string; onPress: () => void }) {
  return (
    <Pressable onPress={onPress} style={({ pressed }) => [styles.orangeButton, pressed && styles.pressed]}>
      <Text style={styles.orangeButtonText}>{label}</Text>
    </Pressable>
  );
}

function FooterLinks() {
  return (
    <>
      <View style={styles.termsRow}>
        <Text style={styles.authLink}>Terms of Service</Text><Text style={styles.authPipe}>|</Text><Text style={styles.authLink}>Privacy Policy</Text>
      </View>
      <Text style={styles.version}>VERSION 2.0.0 | © Copyright of Kasikili Virtual Gaming cc | 2025</Text>
    </>
  );
}

function LoginScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  const [remember, setRemember] = useState(true);
  return (
    <SafeAreaView style={styles.authRoot}>
      <AppStatusBar />
      <ScrollView contentContainerStyle={styles.loginContent} keyboardShouldPersistTaps="handled">
        <AuthHeader />
        <View style={styles.loginFields}>
          <AuthField label="MOBILE NUMBER" />
          <AuthField label="PASSWORD" secure />
        </View>
        <View style={styles.rememberRow}>
          <Text style={styles.rememberText}>REMEMBER PASSWORD ?</Text>
          <Switch value={remember} onValueChange={setRemember} trackColor={{ false: '#d9d9d9', true: '#d9d9d9' }} thumbColor={remember ? '#10dc53' : '#aeb4b6'} style={styles.miniSwitch} />
          <Text style={styles.rememberText}>[ NO | YES ]</Text>
        </View>
        <OrangeButton label="SIGN IN" onPress={() => navigate('game')} />
        <View style={styles.authActions}>
          <Pressable onPress={() => navigate('signup')}><Text style={styles.authLink}>SIGN UP HERE</Text></Pressable>
          <Text style={styles.authPipe}>|</Text>
          <Pressable onPress={() => navigate('forgot')}><Text style={styles.authLink}>FORGOT PASSWORD</Text></Pressable>
        </View>
        <Text style={[styles.authLink, styles.contactAdmin]}>CLICK CONTACT ADMIN</Text>
        <FooterLinks />
      </ScrollView>
    </SafeAreaView>
  );
}

function SignupScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  const [adult, setAdult] = useState(false);
  return (
    <SafeAreaView style={styles.authRoot}>
      <AppStatusBar />
      <ScrollView contentContainerStyle={styles.signupContent} keyboardShouldPersistTaps="handled">
        <AuthHeader compact />
        <View style={styles.signupFields}>
          <AuthField label="REFERRAL CODE" />
          <AuthField label="REGION" />
          <AuthField label="MOBILE NUMBER" />
          <AuthField label="PASSWORD" secure />
        </View>
        <Pressable onPress={() => setAdult((value) => !value)} style={styles.consentBox}>{adult && <View style={styles.consentCheck} />}</Pressable>
        <Text style={styles.consentText}>I HEREBY CONFIRM THAT I AM A CONSENTING ADULT ABOVE 18 YEARS OF AGE.</Text>
        <OrangeButton label="SIGN UP" onPress={() => navigate('login')} />
        <View style={styles.authActions}>
          <Pressable onPress={() => navigate('login')}><Text style={styles.authLink}>LOGIN HERE</Text></Pressable>
          <Text style={styles.authPipe}>|</Text>
          <Pressable onPress={() => navigate('forgot')}><Text style={styles.authLink}>FORGOT PASSWORD</Text></Pressable>
        </View>
        <FooterLinks />
      </ScrollView>
    </SafeAreaView>
  );
}

function ForgotScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  return (
    <SafeAreaView style={styles.authRoot}>
      <AppStatusBar />
      <ScrollView contentContainerStyle={styles.loginContent} keyboardShouldPersistTaps="handled">
        <AuthHeader />
        <Text style={styles.forgotHeading}>FORGOT PASSWORD</Text>
        <Text style={styles.forgotCopy}>Enter your registered mobile number and we will send you a password reset code.</Text>
        <AuthField label="MOBILE NUMBER" />
        <OrangeButton label="RESET PASSWORD" onPress={() => navigate('login')} />
        <Pressable onPress={() => navigate('login')}><Text style={[styles.authLink, styles.forgotBack]}>BACK TO SIGN IN</Text></Pressable>
        <FooterLinks />
      </ScrollView>
    </SafeAreaView>
  );
}

function DigitDisplay({ value }: { value: number }) {
  return (
    <View style={styles.digitDisplay}>
      {String(value).padStart(4, '0').split('').map((digit, index) => <Text key={`${digit}-${index}`} style={styles.digit}>{digit}</Text>)}
    </View>
  );
}

function WheelAssembly({ rotation, win, credit }: { rotation: Animated.AnimatedInterpolation<string>; win: number; credit: number }) {
  return (
    <View style={styles.wheelAssembly}>
      <Image source={art.win} resizeMode="stretch" style={styles.winPanel} />
      <Image source={art.credit} resizeMode="stretch" style={styles.creditPanel} />
      <View style={styles.winDigits}><DigitDisplay value={win} /></View>
      <View style={styles.creditDigits}><DigitDisplay value={credit} /></View>
      <Animated.Image source={art.wheel} resizeMode="contain" style={[styles.gameWheel, { transform: [{ rotate: rotation }] }]} />
    </View>
  );
}

function BetNumber({
  number,
  selectedNumber,
  selectedStake,
  onSelect,
}: {
  number: number;
  selectedNumber: number | null;
  selectedStake: number;
  onSelect: (number: number, stake: number) => void;
}) {
  const red = redNumbers.has(number);
  const numberSelected = selectedNumber === number;
  return (
    <View style={styles.numberTile}>
      <Pressable
        accessibilityLabel={`Select number ${number}`}
        onPress={() => onSelect(number, selectedStake || 1)}
        style={[styles.numberCircle, number === 0 ? styles.greenNumber : red ? styles.redNumber : styles.blackNumber, numberSelected && styles.selectedNumberCircle]}
      >
        <Text style={[styles.numberLabel, number >= 10 && styles.twoDigitNumber]}>{number}</Text>
      </Pressable>
      {['1', '2', '3', '4'].map((value, index) => (
        <Pressable
          accessibilityLabel={`Bet ${value} tokens on ${number}`}
          key={value}
          onPress={() => onSelect(number, Number(value))}
          style={({ pressed }) => [
            styles.betSquare,
            index % 2 === 0 ? styles.betLeft : styles.betRight,
            index < 2 ? styles.betTop : styles.betBottom,
            numberSelected && selectedStake === Number(value) && styles.selectedBet,
            pressed && styles.betPressed,
          ]}
        >
          <Text style={styles.betSquareText}>{value}</Text>
        </Pressable>
      ))}
    </View>
  );
}

function GameMenu({ close, navigate }: { close: () => void; navigate: (screen: Screen) => void }) {
  return (
    <View style={styles.menuOverlay}>
      <View style={styles.menuCard}>
        <Pressable onPress={() => navigate('account')}><Text style={styles.menuItem}>CASH OUT</Text></Pressable>
        <Pressable onPress={() => navigate('account')}><Text style={styles.menuItem}>BUY CREDITS</Text></Pressable>
        <Pressable onPress={() => navigate('leaderboard')}><Text style={styles.menuItem}>LEADERBOARD</Text></Pressable>
        <Pressable onPress={() => navigate('notifications')}><Text style={styles.menuItem}>NOTIFICATIONS</Text></Pressable>
        <Pressable onPress={() => navigate('login')}><Text style={styles.menuItem}>SIGN OUT</Text></Pressable>
      </View>
      <Pressable accessibilityRole="button" accessibilityLabel="Close menu" onPress={close} style={styles.menuDismissArea} />
    </View>
  );
}

function GameScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  const { width } = useWindowDimensions();
  const scale = Math.min(width / 375, 1.18);
  const spin = useRef(new Animated.Value(0)).current;
  const [menu, setMenu] = useState(false);
  const [credit, setCredit] = useState(0);
  const [win, setWin] = useState(0);
  const [selectedNumber, setSelectedNumber] = useState<number | null>(null);
  const [selectedStake, setSelectedStake] = useState(1);
  const rotation = spin.interpolate({ inputRange: [0, 1], outputRange: ['0deg', '2160deg'] });
  const start = () => {
    if (selectedNumber === null) return;
    spin.setValue(0);
    setCredit((value) => value + selectedStake);
    setWin(0);
    Animated.timing(spin, { toValue: 1, duration: 2600, useNativeDriver: true }).start(() => setWin(Math.random() > 0.7 ? 10 : 0));
  };
  return (
    <SafeAreaView style={styles.gameRoot}>
      <AppStatusBar />
      <ScrollView bounces={false} showsVerticalScrollIndicator={false} contentContainerStyle={styles.gamePage}>
        <View style={{ width: 375 * scale, height: 783 * scale }}>
          <View style={{ width: 375, height: 783, paddingTop: 20, transform: [{ scale }], transformOrigin: 'top left' } as never}>
            <WheelAssembly rotation={rotation} win={win} credit={credit} />
            <Text style={styles.gameHeading}>KASIKILI BERGMANN ROULETTE</Text>
            <View style={styles.gameControls}>
              <View style={styles.controlMenu}><Hamburger onPress={() => setMenu(true)} /></View>
              <View style={styles.multiplierRow}>{multipliers.map((value) => <Text key={value} style={styles.multiplier}>{value}</Text>)}</View>
              <View style={styles.actionRow}>
                <Pressable style={styles.cancelButton} onPress={() => { setWin(0); setSelectedNumber(null); setSelectedStake(1); }}><Text style={styles.gameButtonText}>CANCEL</Text></Pressable>
                <Pressable style={styles.startButton} onPress={start}><Text style={[styles.gameButtonText, styles.startText]}>START</Text></Pressable>
              </View>
            </View>
            <View style={styles.betTable}>
              <View style={styles.tableRow}><View style={styles.numberTile} /><BetNumber number={0} selectedNumber={selectedNumber} selectedStake={selectedStake} onSelect={(number, stake) => { setSelectedNumber(number); setSelectedStake(stake); }} /><View style={styles.numberTile} /></View>
              {numberRows.map((row) => <View key={row.join('-')} style={styles.tableRow}>{row.map((number) => <BetNumber key={number} number={number} selectedNumber={selectedNumber} selectedStake={selectedStake} onSelect={(nextNumber, stake) => { setSelectedNumber(nextNumber); setSelectedStake(stake); }} />)}</View>)}
            </View>
          </View>
        </View>
      </ScrollView>
      {menu && <GameMenu close={() => setMenu(false)} navigate={navigate} />}
    </SafeAreaView>
  );
}

function AccountScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  const [amount, setAmount] = useState('0');
  return (
    <SafeAreaView style={styles.whiteRoot}>
      <AppStatusBar />
      <ScrollView contentContainerStyle={styles.accountPage}>
        <View style={styles.whiteHeader}><BackIcon onPress={() => navigate('game')} /><Text style={styles.whiteHeaderTitle}>ACCOUNT</Text><View style={styles.backButton} /></View>
        <View style={styles.accountInfo}>
          <Text style={styles.infoLabel}>Mobile Number</Text><Text style={styles.infoValue}>0857430513</Text>
          <Text style={styles.infoLabel}>Region</Text><Text style={styles.infoValue}>Khomas</Text>
          <Text style={styles.infoLabel}>UID</Text><Text style={styles.infoValue}>U1405</Text>
        </View>
        <View style={styles.sectionRule} />
        <View style={styles.balanceRow}><Text style={styles.accountHeading}>BALANCE (NAD)</Text><Text style={styles.redAmount}>1300</Text></View>
        <View style={styles.sectionRule} />
        <Text style={styles.accountHeading}>CASH OUT</Text>
        <TextInput value={amount} onChangeText={setAmount} keyboardType="numeric" style={styles.cashInput} />
        <Pressable style={styles.cashButton}><Text style={styles.cashButtonText}>CASHOUT</Text></Pressable>
        <Text style={styles.cashNote}>You will receive a mobile payment to your registered number.{`\n`}| Daily maximum is N$5000 - Minimum is N$100{`\n`}| Payouts every hour from 08:00 AM - 02:00 AM</Text>
        <View style={styles.sectionRule} />
        <View style={styles.referralRow}>
          <Pressable style={styles.shareButton}><Text style={styles.shareText}>SHARE{`\n`}REFERRAL{`\n`}LINK</Text><View style={styles.shareCircle}><Text style={styles.shareGlyph}>↗</Text></View></Pressable>
          <Text style={styles.referralCopy}>Receive 5 FREE Tokens for each{`\n`}time referral tops up their account,{`\n`}AS LONG as you have a CASH-IN{`\n`}transaction in your last 25{`\n`}transactions</Text>
        </View>
        <View style={styles.sectionRule} />
        <Text style={styles.mutedHeading}>AWAITING PAYOUT:</Text>
        <View style={styles.balanceRow}><Text style={styles.accountHeading}>CREDITS ( N$)</Text><Text style={styles.redAmount}>850</Text></View>
        <View style={styles.sectionRule} />
        <Text style={styles.mutedHeading}>HISTORY : (Last fifteen (25) transactions only)</Text>
        <View style={styles.historyHeader}><Text>ACTIVITY</Text><Text>DATE</Text><Text>DISTRIBUTOR</Text><Text>AMOUNT (N$)</Text></View>
        {[
          ['CASH IN', '02-FEB-22', 'GR456', '1200'], ['CASH OUT', '02-FEB-22', 'YT678', '300'], ['CASH OUT', '02-FEB-22', 'GP856', '900'],
          ['CASH IN', '02-FEB-22', 'JYR456', '650'], ['REFERRAL', '02-FEB-22', 'H4R44', '2500'], ['CASH IN', '02-FEB-22', 'JYR456', '650'],
        ].map((row, index) => <View key={index} style={styles.historyRow}>{row.map((cell) => <Text key={cell} style={styles.historyCell}>{cell}</Text>)}</View>)}
      </ScrollView>
    </SafeAreaView>
  );
}

const notificationSettings = [
  'SHOW NOTIFICATIONS', 'ALLOW FLOATING NOTIFICATION', 'ALLOW LOCK SCREEN NOTIFICATION', 'ALLOW LOCK SCREEN NOTIFICATION',
  'ALLOW NOTIFICATION SOUNDS', 'ALLOW VIBRATIONS', 'ALL USERS ACTIVITY\nNOTIFICATIONS', 'MY ACCOUNT NOTIFICATIONS ONLY',
];

function NotificationsScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  const [values, setValues] = useState(notificationSettings.map(() => true));
  return (
    <SafeAreaView style={styles.whiteRoot}>
      <AppStatusBar />
      <ScrollView contentContainerStyle={styles.notificationsPage}>
        <View style={styles.whiteHeader}><BackIcon onPress={() => navigate('game')} /><Text style={styles.notificationTitle}>PUSH NOTIFICATIONS</Text><View style={styles.backButton} /></View>
        <View style={styles.sectionRule} />
        {notificationSettings.map((label, index) => (
          <View key={`${label}-${index}`} style={[styles.notificationRow, index === 0 && styles.notificationPrimary, (index === 5 || index === 7) && styles.notificationSectionEnd]}>
            <Text style={styles.notificationLabel}>{label}</Text>
            <Switch value={values[index]} onValueChange={(next) => setValues((current) => current.map((value, i) => i === index ? next : value))} trackColor={{ false: '#d7d7d7', true: '#d7d7d7' }} thumbColor={values[index] ? '#10dd54' : '#a9adae'} />
          </View>
        ))}
      </ScrollView>
    </SafeAreaView>
  );
}

function Trend({ value }: { value: 'up' | 'down' | 'flat' }) {
  return <Text style={[styles.trend, value === 'up' ? styles.trendUp : value === 'down' ? styles.trendDown : styles.trendFlat]}>{value === 'up' ? '▲' : value === 'down' ? '▼' : '−'}</Text>;
}

function LeaderInfo() {
  return (
    <View style={styles.moveUpSection}>
      <View style={styles.moveTitleRow}><View><Text style={styles.activeLabel}>Current Active Referrals</Text><View style={styles.activeBox}><Text style={styles.activeValue}>245</Text></View></View><View style={styles.verticalRule} /><Text style={styles.moveTitle}>HOW MOVE UP{`\n`}THE BOARD</Text></View>
      <View style={styles.leaderRule} />
      <Text style={styles.moveCopy}>To be featured in the TOP 10 of the Kasikili Leadership Board Challenge and stand a chance to winning cash prizes on a monthly basis, make sure to :{`\n\n`}  • Login and use the app daily{`\n`}  • Top up your account often{`\n`}  • Refer as many users using your referral link as you can{`\n\n`}The value of your Top ups count and the value of your referrals top ups as well.{`\n\n`}** NB - You ALSO get 5 Tokens each time your Referrals Top up for life!</Text>
    </View>
  );
}

function ExplainerModal({ visible, close }: { visible: boolean; close: () => void }) {
  return (
    <Modal visible={visible} transparent animationType="fade" onRequestClose={close}>
      <View style={styles.explainerShade}>
        <ScrollView contentContainerStyle={styles.explainerScroll}>
          <View style={styles.explainerCard}>
            <Text style={styles.explainerHeading}>LEADERSHIP CHALLENGE</Text>
            <Text style={styles.explainerText}>Top 10 players on the leader board are paid out a sum of cash directly to their PayPulse on their registered numbers.{`\n\n`}Factors listed on the bottom are taken into consideration and weighted by the system to calculate an overall percentage.{`\n\n`}The player with the highest percentage at the end of each month, wins the highest monthly price and the leaderboard resets again on the 1st of each month.</Text>
            <Text style={styles.prizeNote}>Prize value is in N$ ( Namibian Dollars)</Text>
            {[['Total Prize', '5000'], ['1st Place', '2000'], ['2nd Place', '1000'], ['3rd Place', '600'], ['4th – 10th Place', '200']].map(([label, value]) => <View key={label} style={styles.prizeRow}><Text style={styles.prizeLabel}>{label}</Text><View style={styles.prizeBox}><Text style={styles.prizeValue}>{value}</Text></View></View>)}
            <Text style={styles.explainerText}>The value of the cash prizes will change and grow in accordance with the growth of the app, the players and the activity on the platform.{`\n\n`}Kasikili Virtual Gaming cc and its owners reserve the full right to determine any and all amounts of the Jackpot of the Leadership Challenge for its sustainability.{`\n\n`}All payments for winners shall be made between the 1st - 2nd of each month to their registered PayPulse numbers.{`\n\n`}All winners will be made public to all users via push notifications for transparency, with some elements of their numbers hidden for privacy.</Text>
            <Pressable onPress={close} style={styles.closeExplainer}><Text style={styles.closeExplainerText}>CLOSE</Text></Pressable>
          </View>
        </ScrollView>
      </View>
    </Modal>
  );
}

function LeaderboardScreen({ navigate }: { navigate: (screen: Screen) => void }) {
  const [info, setInfo] = useState(false);
  return (
    <SafeAreaView style={styles.leaderRoot}>
      <AppStatusBar />
      <ScrollView contentContainerStyle={styles.leaderPage} showsVerticalScrollIndicator={false}>
        <View style={styles.leaderHeader}><BackIcon onPress={() => navigate('game')} light /><Text style={styles.leaderTitle}>Leaderboard</Text><Text style={styles.shareIcon}>⌯</Text></View>
        <Pressable onPress={() => setInfo(true)} style={styles.crownWrap}><Image source={art.crown} resizeMode="contain" style={styles.leaderCrown} /><Text style={styles.bigPosition}>148</Text></Pressable>
        <View style={styles.leaderTools}><View><Text style={styles.activeLabelLight}>Active Referrals</Text><View style={styles.activeBox}><Text style={styles.activeValue}>245</Text></View><Pressable style={styles.downloadButton}><Text style={styles.downloadText}>DOWNLOAD</Text></Pressable></View><Pressable style={styles.refreshButton}><Text style={styles.refreshText}>Refresh</Text></Pressable></View>
        <Text style={styles.positionTitle}>YOUR POSITION</Text>
        <View style={styles.leaderList}>
          {leaders.map(([rank, mobile, score, trend, tone]) => <View key={rank} style={[styles.leaderRow, tone === 'gold' ? styles.rowGold : tone === 'white' ? styles.rowWhite : styles.rowDark]}><Trend value={trend} /><Text style={[styles.leaderRank, tone === 'dark' && styles.rowDarkText]}>{rank}</Text><Text style={[styles.leaderMobile, tone === 'dark' && styles.rowDarkText]}>{mobile}</Text><Text style={[styles.leaderScore, tone === 'dark' && styles.rowDarkText]}>{score}</Text></View>)}
        </View>
        <View style={styles.leaderRule} />
        <View style={styles.youRow}><Trend value="up" /><Text style={styles.youRank}>148</Text><View style={styles.userCircle}><Text style={styles.userIcon}>♟</Text></View><Text style={styles.youText}>You</Text><Text style={styles.youScore}>2%</Text></View>
        <LeaderInfo />
      </ScrollView>
      <ExplainerModal visible={info} close={() => setInfo(false)} />
    </SafeAreaView>
  );
}

export default function App() {
  const [screen, setScreen] = useState<Screen>('login');
  if (screen === 'login') return <LoginScreen navigate={setScreen} />;
  if (screen === 'signup') return <SignupScreen navigate={setScreen} />;
  if (screen === 'forgot') return <ForgotScreen navigate={setScreen} />;
  if (screen === 'game') return <GameScreen navigate={setScreen} />;
  if (screen === 'account') return <AccountScreen navigate={setScreen} />;
  if (screen === 'notifications') return <NotificationsScreen navigate={setScreen} />;
  return <LeaderboardScreen navigate={setScreen} />;
}

const styles = StyleSheet.create({
  pressed: { opacity: 0.7 },
  authRoot: { flex: 1, backgroundColor: '#fff' },
  whiteRoot: { flex: 1, backgroundColor: '#fff' },
  loginContent: { minHeight: 780, alignItems: 'center', paddingHorizontal: 21, paddingTop: 42, paddingBottom: 18 },
  signupContent: { minHeight: 780, alignItems: 'center', paddingHorizontal: 21, paddingTop: 42, paddingBottom: 18 },
  authTitle: { fontSize: 14, fontWeight: '900', color: '#070707', marginBottom: 23 },
  authRule: { alignSelf: 'stretch', height: 1, backgroundColor: '#777', marginBottom: 10 },
  authWheel: { width: 184, height: 184, marginTop: 38 },
  authWheelCompact: { width: 176, height: 176, marginTop: 2 },
  loginFields: { width: '100%', gap: 24, marginTop: 62 },
  signupFields: { width: '100%', gap: 18, marginTop: 11 },
  authField: { width: '100%', height: 59, borderRadius: 20, backgroundColor: '#9bd3a3', color: '#fff', textAlign: 'center', fontSize: 14, paddingHorizontal: 18 },
  rememberRow: { height: 39, flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 5 },
  rememberText: { fontSize: 7, fontWeight: '800', color: '#111' },
  miniSwitch: { transform: [{ scaleX: 0.58 }, { scaleY: 0.58 }], marginHorizontal: -8 },
  orangeButton: { width: '88%', height: 48, borderRadius: 10, backgroundColor: '#f88b05', alignItems: 'center', justifyContent: 'center', shadowColor: '#000', shadowOpacity: 0.28, shadowRadius: 3, shadowOffset: { width: 0, height: 3 }, elevation: 4 },
  orangeButtonText: { color: '#fff', fontWeight: '800', fontSize: 14 },
  authActions: { flexDirection: 'row', alignItems: 'center', justifyContent: 'center', marginTop: 11 },
  authLink: { color: '#006b8a', fontSize: 10 },
  authPipe: { marginHorizontal: 7, color: '#555' },
  contactAdmin: { fontSize: 12, fontWeight: '700', marginTop: 12 },
  termsRow: { flexDirection: 'row', marginTop: 42 },
  version: { fontSize: 6.5, fontWeight: '600', marginTop: 24 },
  consentBox: { width: 29, height: 18, borderWidth: 1, borderColor: '#111', backgroundColor: '#ddd', marginTop: 11, alignItems: 'center', justifyContent: 'center' },
  consentCheck: { width: 21, height: 12, backgroundColor: '#18d954' },
  consentText: { color: '#00698a', fontSize: 8, marginVertical: 7 },
  forgotHeading: { fontSize: 20, fontWeight: '900', marginTop: 37 },
  forgotCopy: { width: '88%', textAlign: 'center', color: '#006b8a', fontSize: 12, lineHeight: 18, marginVertical: 17 },
  forgotBack: { marginTop: 16, fontWeight: '700' },
  gameRoot: { flex: 1, backgroundColor: '#003f27' },
  gamePage: { alignItems: 'center', minHeight: 770 },
  wheelAssembly: { width: 375, height: 182, position: 'relative', overflow: 'hidden' },
  winPanel: { position: 'absolute', left: 4, top: 15, width: 177, height: 194 },
  creditPanel: { position: 'absolute', right: 4, top: 15, width: 177, height: 194 },
  gameWheel: { position: 'absolute', left: 92, top: 0, width: 191, height: 191 },
  winDigits: { position: 'absolute', left: 16, top: 76 },
  creditDigits: { position: 'absolute', right: 16, top: 76 },
  digitDisplay: { width: 69, height: 25, borderWidth: 3, borderColor: '#ff1720', backgroundColor: '#b21419', flexDirection: 'row', alignItems: 'center', justifyContent: 'space-evenly' },
  digit: { color: '#fff', fontSize: 17, fontWeight: '900', fontFamily: 'monospace' },
  gameHeading: { color: '#fff', fontFamily: 'serif', textDecorationLine: 'underline', fontSize: 16, textAlign: 'center', height: 25 },
  gameControls: { height: 74, position: 'relative' },
  controlMenu: { position: 'absolute', left: 6, top: -1 },
  hamburger: { width: 36, height: 36, borderRadius: 18, backgroundColor: '#ced900', alignItems: 'center', justifyContent: 'center', gap: 3 },
  hamburgerLine: { width: 22, height: 3, borderRadius: 2, backgroundColor: '#183d27' },
  multiplierRow: { flexDirection: 'row', justifyContent: 'center' },
  multiplier: { height: 39, minWidth: 41, paddingHorizontal: 8, borderWidth: 1, borderColor: '#fff', backgroundColor: '#9d1014', color: '#fff', fontFamily: 'serif', fontSize: 17, textAlign: 'center', textAlignVertical: 'center' },
  actionRow: { flexDirection: 'row', justifyContent: 'space-between', paddingHorizontal: 88, marginTop: 5 },
  cancelButton: { width: 58, height: 30, borderRadius: 6, backgroundColor: '#ced900', alignItems: 'center', justifyContent: 'center' },
  startButton: { width: 58, height: 30, borderRadius: 6, backgroundColor: '#9c1014', alignItems: 'center', justifyContent: 'center' },
  gameButtonText: { fontFamily: 'serif', color: '#050505', fontSize: 11 },
  startText: { color: '#fff' },
  betTable: { width: 355, height: 482, borderWidth: 1, borderColor: '#507a67', alignSelf: 'center' },
  tableRow: { flexDirection: 'row', height: 96 },
  numberTile: { flex: 1, height: 96, borderRightWidth: 1, borderBottomWidth: 1, borderColor: '#507a67', alignItems: 'center', justifyContent: 'center', position: 'relative' },
  numberCircle: { width: 53, height: 44, borderRadius: 27, alignItems: 'center', justifyContent: 'center' },
  greenNumber: { backgroundColor: '#0f7a46' }, redNumber: { backgroundColor: '#b8141b' }, blackNumber: { backgroundColor: '#020202' },
  numberLabel: { color: '#fff', fontSize: 27, fontWeight: '800' },
  twoDigitNumber: { fontSize: 23 },
  selectedNumberCircle: { borderWidth: 3, borderColor: '#fff43c', shadowColor: '#fff43c', shadowOpacity: 0.9, shadowRadius: 7, elevation: 8 },
  betSquare: { position: 'absolute', width: 40, height: 32, borderRadius: 4, backgroundColor: '#ccd900', alignItems: 'center', justifyContent: 'center' },
  betLeft: { left: 8 }, betRight: { right: 8 }, betTop: { top: 8 }, betBottom: { bottom: 8 }, betPressed: { backgroundColor: '#fff83a' },
  selectedBet: { backgroundColor: '#fff43c', borderWidth: 3, borderColor: '#ff8a00', shadowColor: '#fff43c', shadowOpacity: 1, shadowRadius: 7, elevation: 8 },
  betSquareText: { fontSize: 14, fontWeight: '800', color: '#050505' },
  menuOverlay: { ...StyleSheet.absoluteFillObject, backgroundColor: 'rgba(0,35,21,0.16)', alignItems: 'center', justifyContent: 'center' },
  menuCard: { width: 283, paddingVertical: 23, borderRadius: 25, backgroundColor: 'rgba(89,180,91,0.9)', alignItems: 'center', gap: 12 },
  menuItem: { color: '#fff', fontFamily: 'serif', fontWeight: '900', fontSize: 26, lineHeight: 45, textShadowColor: 'rgba(0,0,0,.18)', textShadowRadius: 2 },
  menuDismissArea: { position: 'absolute', left: 0, top: 205, width: 58, height: 58, backgroundColor: 'transparent' },
  backButton: { width: 45, height: 45, position: 'relative' },
  backDoor: { position: 'absolute', right: 1, top: 6, width: 25, height: 31, borderWidth: 2, borderColor: '#ef383d', borderRadius: 3 },
  backDoorLight: { borderColor: '#ef383d' },
  backArrow: { position: 'absolute', left: 0, top: 5, color: '#ef383d', fontSize: 30 },
  backArrowLight: { color: '#ef383d' },
  whiteHeader: { width: '100%', height: 70, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingHorizontal: 8 },
  whiteHeaderTitle: { fontSize: 26, fontWeight: '900' },
  accountPage: { paddingHorizontal: 21, paddingBottom: 30 },
  accountInfo: { display: 'flex', flexDirection: 'row', flexWrap: 'wrap' },
  infoLabel: { width: '40%', fontSize: 17, lineHeight: 25 }, infoValue: { width: '60%', fontSize: 17, lineHeight: 25 },
  sectionRule: { height: 1, backgroundColor: '#5f5f5f', width: '100%', marginVertical: 6 },
  balanceRow: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center' },
  accountHeading: { fontSize: 23, color: '#111' }, redAmount: { color: '#e33d42', fontSize: 29, fontWeight: '800' },
  cashInput: { width: '92%', alignSelf: 'center', height: 42, borderWidth: 1, borderRadius: 5, backgroundColor: '#fafafa', textAlign: 'center', color: '#e33d42', fontSize: 24, fontWeight: '700', shadowColor: '#000', shadowOpacity: .25, shadowRadius: 2, shadowOffset: { width: 0, height: 2 }, elevation: 3, marginVertical: 7 },
  cashButton: { width: '62%', height: 37, alignSelf: 'center', backgroundColor: '#9bd3a3', borderRadius: 8, alignItems: 'center', justifyContent: 'center' },
  cashButtonText: { fontWeight: '900', fontSize: 12 }, cashNote: { textAlign: 'center', fontSize: 10, color: '#555', lineHeight: 14, marginVertical: 6 },
  referralRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' },
  shareButton: { width: 148, height: 62, borderRadius: 30, backgroundColor: '#eee', flexDirection: 'row', alignItems: 'center', justifyContent: 'center', shadowColor: '#000', shadowOpacity: .2, shadowRadius: 4, shadowOffset: { width: 0, height: 3 }, elevation: 3 },
  shareText: { fontSize: 10, fontWeight: '800', textAlign: 'center', marginRight: 9 }, shareCircle: { width: 43, height: 43, borderRadius: 22, backgroundColor: '#124b85', alignItems: 'center', justifyContent: 'center', borderWidth: 5, borderColor: '#fff' }, shareGlyph: { color: '#fff', fontSize: 24 },
  referralCopy: { fontSize: 10, fontWeight: '700', textAlign: 'center' }, mutedHeading: { fontSize: 13, color: '#606269', fontWeight: '900' },
  historyHeader: { flexDirection: 'row', justifyContent: 'space-between', borderBottomWidth: 1, paddingVertical: 5 },
  historyRow: { flexDirection: 'row', paddingVertical: 3 }, historyCell: { width: '25%', fontSize: 10, fontWeight: '700', color: '#51545a' },
  notificationsPage: { paddingHorizontal: 21, paddingBottom: 40 }, notificationTitle: { fontSize: 26, fontWeight: '900' },
  notificationRow: { minHeight: 50, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between', paddingHorizontal: 12 },
  notificationPrimary: { minHeight: 96, borderBottomWidth: 1, borderColor: '#666' }, notificationSectionEnd: { borderBottomWidth: 1, borderColor: '#666', paddingBottom: 38, marginBottom: 18 },
  notificationLabel: { maxWidth: '75%', fontSize: 13, fontWeight: '900' },
  leaderRoot: { flex: 1, backgroundColor: '#0a472f' }, leaderPage: { paddingHorizontal: 17, paddingBottom: 30 },
  leaderHeader: { height: 80, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }, leaderTitle: { color: '#fff', fontSize: 23, fontWeight: '600' }, shareIcon: { color: '#fff', fontSize: 35 },
  crownWrap: { alignSelf: 'center', width: 238, height: 307, alignItems: 'center' }, leaderCrown: { width: 238, height: 307 }, bigPosition: { position: 'absolute', top: 145, color: '#000', fontSize: 73, fontWeight: '900', transform: [{ rotate: '-5deg' }] },
  leaderTools: { minHeight: 113, flexDirection: 'row', justifyContent: 'space-between', alignItems: 'flex-start' },
  activeLabelLight: { color: '#fff', fontSize: 11 }, activeLabel: { color: '#fff', fontSize: 9 }, activeBox: { width: 96, height: 42, borderRadius: 10, backgroundColor: '#d4c5e1', borderWidth: 5, borderColor: '#ffc62c', alignItems: 'center', justifyContent: 'center' }, activeValue: { fontSize: 14, fontWeight: '900' },
  downloadButton: { marginTop: 11, width: 91, height: 32, backgroundColor: '#ffc62c', borderRadius: 5, alignItems: 'center', justifyContent: 'center' }, downloadText: { fontSize: 10, fontWeight: '900' },
  refreshButton: { width: 88, height: 32, backgroundColor: '#ffc62c', borderRadius: 5, alignItems: 'center', justifyContent: 'center' }, refreshText: { fontWeight: '900' },
  positionTitle: { color: '#fff', fontSize: 18, textAlign: 'center', marginVertical: 8 }, leaderList: { gap: 14 },
  leaderRow: { height: 65, borderRadius: 20, flexDirection: 'row', alignItems: 'center', paddingHorizontal: 20 }, rowGold: { backgroundColor: '#ffc62c' }, rowWhite: { backgroundColor: '#f4f4f4' }, rowDark: { backgroundColor: '#315846' },
  rowDarkText: { color: '#fff' }, trend: { width: 25, fontSize: 16 }, trendUp: { color: '#21e981' }, trendDown: { color: '#f33d45' }, trendFlat: { color: '#f52ec3' },
  leaderRank: { width: 31, fontSize: 14 }, leaderMobile: { flex: 1, fontSize: 16, fontWeight: '700' }, leaderScore: { fontSize: 28, fontWeight: '900' },
  leaderRule: { height: 1, backgroundColor: '#fff', marginVertical: 16 }, youRow: { height: 65, borderRadius: 33, backgroundColor: '#fff', flexDirection: 'row', alignItems: 'center', paddingHorizontal: 20 },
  youRank: { color: '#ff20c4', width: 40 }, userCircle: { width: 40, height: 40, borderRadius: 20, backgroundColor: '#36244e', alignItems: 'center', justifyContent: 'center' }, userIcon: { color: '#fff', fontSize: 22 }, youText: { color: '#ff20c4', fontSize: 17, marginLeft: 19, flex: 1 }, youScore: { color: '#ff20c4', fontSize: 28, fontWeight: '900' },
  moveUpSection: { marginTop: 13 }, moveTitleRow: { flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }, verticalRule: { width: 1, height: 50, backgroundColor: '#fff' }, moveTitle: { color: '#fff', fontSize: 27, lineHeight: 29, textAlign: 'center' }, moveCopy: { color: '#fff', fontSize: 12.5, lineHeight: 15 },
  explainerShade: { flex: 1, backgroundColor: 'rgba(3,52,34,.65)' }, explainerScroll: { paddingHorizontal: 28, paddingVertical: 150 }, explainerCard: { borderRadius: 22, backgroundColor: 'rgba(54,135,65,.94)', paddingHorizontal: 18, paddingVertical: 70, alignItems: 'center' },
  explainerHeading: { color: '#fff', fontFamily: 'serif', fontWeight: '900', fontSize: 18, marginBottom: 24 }, explainerText: { color: '#fff', fontSize: 15, lineHeight: 21, textAlign: 'center' }, prizeNote: { color: '#fff', fontFamily: 'serif', fontWeight: '900', fontSize: 15, marginVertical: 18 },
  prizeRow: { width: '100%', minHeight: 76, flexDirection: 'row', alignItems: 'center', justifyContent: 'space-between' }, prizeLabel: { color: '#fff', fontFamily: 'serif', fontWeight: '900', fontSize: 18, textAlign: 'center', flex: 1 }, prizeBox: { width: 126, height: 49, borderRadius: 12, backgroundColor: '#ddd', alignItems: 'center', justifyContent: 'center' }, prizeValue: { fontSize: 25, fontWeight: '900' },
  closeExplainer: { marginTop: 30, width: 130, height: 42, borderRadius: 10, backgroundColor: '#ffc62c', alignItems: 'center', justifyContent: 'center' }, closeExplainerText: { fontWeight: '900' },
});
