const router = require('express').Router();
const jwt = require('jsonwebtoken');
const config1 = require('../helper/config');
const middlewares = require('../helper/middlewares');

const auth = require('../controller/authentication');
const OTP = require('../controller/sendOtp');
const usertype = require('../controller/User');

const general = require('../controller/general');
const credit = require('../controller/credit_balance');
const distributor = require('../controller/distributor');
const transcation = require('../controller/transcation');

const emailnoti = require('../helper/emailnotification');

const rolemodule = require('../controller/module');
const configdata = require('../controller/site_config');
const encryption = require('../controller/encypt_decypt');

// Authentication
router.post('/adminlogin', middlewares.routeDecryptMiddleWares, auth.adminlogin);
router.post('/login', middlewares.routeDecryptMiddleWares, auth.login);
router.post('/register', middlewares.routeDecryptMiddleWares, auth.register);
router.post('/checkmobilenumber', middlewares.routeDecryptMiddleWares, auth.checkmobilenumber);
router.post('/checkemail', middlewares.routeDecryptMiddleWares, auth.checkemail);
router.post('/forgetPassword', middlewares.routeDecryptMiddleWares, auth.forgetPassword);

router.post('/registerotp', middlewares.routeDecryptMiddleWares, OTP.sendRegistrationOtp);
router.post('/sendotp', middlewares.routeDecryptMiddleWares, OTP.otpSend);
router.post('/verifyotp', middlewares.routeDecryptMiddleWares, OTP.verifyOtp);

router.post('/changePassword', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, auth.changePassword);
router.post('/updateUserFCMToken', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, auth.updateUserFCMToken);

router.post('/logout', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, auth.logout);

// User
router.post('/addusertype', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.createusertype);
router.post('/createadmin', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.createadmin);
router.post('/creatdistributor', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.creatdistributo);
router.post('/resetpassworddistributor', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.resetdistributorpassword);
router.post('/changestatusdistributor', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.changestatusdistributor);
router.post('/changestatususer', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.changestatususer);
router.post('/getallusers', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.getallusers);
router.post('/getallusersdownload', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.getallusersdownload);
router.get('/getuserdata/:id', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.getuserdata);
router.get('/getusertranscation/:id', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.getusertranscation);
router.get('/getprofiledata', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.getprofiledata);
router.get('/getmoduleaccess', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, rolemodule.getmoduleaccess);
router.get('/getuserawaitingcashout', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, usertype.getuserawaitingcashout);
router.post('/updateuserbalance', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.updateuserbalance);
router.get('/getuserbalance', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.getuserbalance);

// Transcation
router.post('/sendcredit', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.sendcredit);
router.post('/sendreq', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.cashout);
router.get('/getusertranscation', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.getusertranscation);
router.get('/getdistributortranscation', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.getdistributortranscation);
router.post('/acceptcashout', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, transcation.acceptcashout);

// Credit
router.post('/generatebalance', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, credit.generatebalance);
router.get('/getavailablecredit', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, credit.getavailablecredit);
router.get('/getalldistributorbalance', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, credit.getalldistributorbalance);
router.get('/getallusersbalance', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, credit.getallusersbalance);
router.post('/transfertodistri', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, credit.transferdistri);

// Distributor
router.post('/getdistributorlist', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, distributor.getalldistributor);
router.get('/getditributordata/:id', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, distributor.getditributordata);
router.get('/getdistranscation/:id', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, distributor.getditributortranscation);
router.post('/downloaddistdata', middlewares.routeDecryptMiddleWares, distributor.downloaddistdata);
router.get('/getdistributorslist', middlewares.routeMiddleWares, distributor.getdistributorslist);
router.get('/getupdatedistributorbalance', middlewares.routeMiddleWares, transcation.getupdatedistributorbalance);

// General
router.get('/getalldistributor', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, general.getalldistributor);
router.get('/getallusers', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, general.getallusers);
router.get('/getallcashreq', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, general.getallcashreq);
router.get('/getallcashrequsers', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, general.getallcashrequsers);

// Module
router.post('/addmodule', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, rolemodule.addmodule);
router.post('/updatemodule', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, rolemodule.updatemodule);
router.get('/getmodule/:id', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, rolemodule.getmodule);
router.get('/getallmodule', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, rolemodule.getallmodule);

// Site-configuration
router.post('/addconfig', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, configdata.configinsert);
router.post('/updateconfig', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, configdata.configupdate);
router.post('/getconfig', middlewares.routeMiddleWares, middlewares.routeDecryptMiddleWares, configdata.getconfig);
router.get('/getallconfig', middlewares.routeMiddleWares, configdata.getallconfig);

// E-mail sent
router.get('/sentemail', emailnoti.sendemail);

// encryption -- decryption
router.post('/encryption', encryption.encrypted);
router.post('/decryption', encryption.decrypted);

module.exports = router;