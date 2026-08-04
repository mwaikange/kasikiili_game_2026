const forever = require('forever-monitor');

const date = new Date();

const filedate = date.getFullYear() +'-'+ (date.getMonth() + 1) +'-'+ date.getDate();

const child = new forever.Monitor('index.js', {
  max: 3,
  silent: false,
  uid: 'index',
  minUptime: 10000,
  spinSleepTime: 5000,
  logFile: `./logs/log(${filedate}).txt`,
  errFile: `./errorlogs/error(${filedate}).txt`
});

child.on('exit', function () {
  console.log('index.js has exited after 5 to 10 seconds restarts');
});

child.start();