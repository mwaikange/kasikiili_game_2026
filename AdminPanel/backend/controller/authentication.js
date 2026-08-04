const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const smtpmail = require('../helper/emailnotification');
const smsnotify = require('../helper/smsnotification');
const jwt = require('jsonwebtoken');
const sha256 = require('crypto-js/sha256');

const cryptojs = require('../helper/crypto');

module.exports.adminlogin = async (req, res) => {
    try {
        await knex('Users')
            .where({ email: req.body.email })
            .then(async (checkUser) => {
                if (checkUser.length > 0) {
                    if(checkUser[0].user_type == 1 || checkUser[0].user_type == 4) {
                        if (checkUser[0].password == sha256(req.body.password).toString()) {
                            if (checkUser[0].is_active == 'N') {
                                res.status(400).send(await middlewares.responseMiddleWares("account_deactivate", false, undefined, 400));
                                return false;
                            }

                            if (checkUser[0].is_delete == 'Y') {
                                res.status(400).send(await middlewares.responseMiddleWares("account_delete", false, undefined, 400));
                                return false;
                            }

                            const token = jwt.sign({ id: checkUser[0].id, user_type: checkUser[0].user_type }, config.secret_key, { expiresIn: '24h' });

                            let user_v = {
                                user_device_token: token,
                                is_login: 'Y',
                                updated_by: checkUser[0].id,
                                updated_at: new Date(),
                            }

                            await knex('user_device')
                                .where({ user_id: checkUser[0].id })
                                .then(async (devicedata) => {
                                    if (devicedata.length > 0) {
                                        await knex('user_device')
                                            .update(user_v)
                                            .where({ user_id: checkUser[0].id })
                                            .then(async (devicedata) => {

                                            })
                                    } else {
                                        user_v['user_id'] = checkUser[0].id;
                                        user_v['created_by'] = checkUser[0].id;
                                        user_v['created_at'] = new Date()

                                        await knex('user_device')
                                            .insert(user_v)
                                            .then(async (devicedata) => {

                                            })
                                    }
                                })

                            let userDetails = {
                                user_id: checkUser[0].id,
                                accesstoken: token,
                                user_type: checkUser[0].user_type
                            }

                            // login success
                            res.status(200).send(await middlewares.responseMiddleWares('login_success', true, userDetails, 200));
                        } else {
                            // login not found
                            res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_2', false, undefined, 400));
                        }
                    } else {
                        res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_2', false, undefined, 400));
                    }
                } else {
                    // login not found
                    res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_1', false, undefined, 400));
                }
            })
    } catch (err) {
        console.log("error", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// Distributor & User login
module.exports.login = async (req, res) => {
    try {
        const distributor_code = req.body.distributor_code || '';

        // Distributor login
        if (distributor_code != null && distributor_code != '' && distributor_code != undefined) {
            await knex('Users as u')
                .select('u.id', 'u.name', 'u.email', 'u.surname', 'u.mobile_number', 'u.is_active', 'u.is_delete', 'info.distributor_code', 'u.password', 'info.credit_balance as balance', 'u.region')
                .leftJoin('users_information as info', 'u.id', 'info.user_id')
                .where({ 'u.mobile_number': req.body.mobile_number, 'info.distributor_code': distributor_code, 'u.user_type': 2 })
                .then(async (checkUser) => {
                    if (checkUser.length > 0) {
                        if (checkUser[0].password == sha256(req.body.password).toString()) {
                            if (checkUser[0].is_active == 'N') {
                                res.status(400).send(await middlewares.responseMiddleWares("account_deactivate", false, undefined, 400));
                                return false;
                            }

                            if (checkUser[0].is_delete == 'Y') {
                                res.status(400).send(await middlewares.responseMiddleWares("account_delete", false, undefined, 400));
                                return false;
                            }

                            const token = jwt.sign({ id: checkUser[0].id, user_type: checkUser[0].user_type }, config.secret_key, { expiresIn: '24h' });

                            let user_v = {
                                user_device_token: token,
                                fcm_token: req.body.fcm_token,
                                user_device_id: req.body.user_device_id,
                                is_login: 'Y',
                                updated_by: checkUser[0].id,
                                updated_at: new Date(),
                            }

                            await knex('user_device')
                                .where({ user_id: checkUser[0].id })
                                .then(async (devicedata) => {
                                    if (devicedata.length > 0) {
                                        await knex('user_device')
                                            .update(user_v)
                                            .where({ user_id: checkUser[0].id })
                                            .then(async (devicedata) => {

                                            })
                                    } else {
                                        user_v['user_id'] = checkUser[0].id;
                                        user_v['created_by'] = checkUser[0].id;
                                        user_v['created_at'] = new Date()

                                        await knex('user_device')
                                            .insert(user_v)
                                            .then(async (devicedata) => {

                                            })
                                    }
                                })

                            let userDetails = {
                                user_id: checkUser[0].id,
                                accesstoken: token,
                                user_device_id: req.body.user_device_id,
                                first_name: checkUser[0].name,
                                last_name: checkUser[0].surname,
                                email: checkUser[0].email,
                                mobile_number: checkUser[0].mobile_number,
                                region: checkUser[0].region,
                                balance: checkUser[0].balance
                            }

                            // login success
                            res.status(200).send(await middlewares.responseMiddleWares('login_success', true, userDetails, 200));
                        } else {
                            // login not found
                            res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_2', false, undefined, 400));
                        }
                    } else {
                        // login not found
                        res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_1', false, undefined, 400));
                    }
                })
        } // user login
        else {
            await knex('Users as u')
                .select('u.id', 'u.name', 'u.email', 'u.surname', 'u.mobile_number', 'u.is_active', 'u.is_delete', 'info.distributor_code', 'u.password', 'info.credit_balance as balance', 'u.region', 'info.winning_balance as winning_balance')
                .leftJoin('users_information as info', 'u.id', 'info.user_id')
                .where({ 'u.mobile_number': req.body.mobile_number, 'u.user_type': 3 })
                .then(async (checkUser) => {
                    if (checkUser.length > 0) {
                        if (checkUser[0].password == sha256(req.body.password).toString()) {
                            if (checkUser[0].is_active == 'N') {
                                res.status(400).send(await middlewares.responseMiddleWares("account_deactivate", false, undefined, 400));
                                return false;
                            }

                            if (checkUser[0].is_delete == 'Y') {
                                res.status(400).send(await middlewares.responseMiddleWares("account_delete", false, undefined, 400));
                                return false;
                            }

                            const token = jwt.sign({ id: checkUser[0].id, user_type: checkUser[0].user_type }, config.secret_key, { expiresIn: '24h' });

                            let user_v = {
                                user_device_token: token,
                                fcm_token: req.body.fcm_token,
                                user_device_id: req.body.user_device_id,
                                is_login: 'Y',
                                updated_by: checkUser[0].id,
                                updated_at: new Date(),
                            }

                            await knex('user_device')
                                .where({ user_id: checkUser[0].id })
                                .then(async (devicedata) => {
                                    if (devicedata.length > 0) {
                                        await knex('user_device')
                                            .update(user_v)
                                            .where({ user_id: checkUser[0].id })
                                            .then(async (devicedata) => {

                                            })
                                    } else {
                                        user_v['user_id'] = checkUser[0].id;
                                        user_v['created_by'] = checkUser[0].id;
                                        user_v['created_at'] = new Date()

                                        await knex('user_device')
                                            .insert(user_v)
                                            .then(async (devicedata) => {

                                            })
                                    }
                                })

                            let userDetails = {
                                user_id: checkUser[0].id,
                                accesstoken: token,
                                user_device_id: req.body.user_device_id,
                                first_name: checkUser[0].name,
                                last_name: checkUser[0].surname,
                                email: checkUser[0].email,
                                mobile_number: checkUser[0].mobile_number,
                                region: checkUser[0].region,
                                balance: checkUser[0].balance,
                                winning_balance: checkUser[0].winning_balance
                            }

                            // login success
                            res.status(200).send(await middlewares.responseMiddleWares('login_success', true, userDetails, 200));
                        } else {
                            // login not found
                            res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_2', false, undefined, 400));
                        }
                    } else {
                        // login not found
                        res.status(400).send(await middlewares.responseMiddleWares('login_incorrect_1', false, undefined, 400));
                    }
                })
        }
    } catch (err) {
        console.log("error", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// User registration
module.exports.register = async (req, res) => {
    try {
        const mobile_number = req.body.mobile_number || '';
        const email = req.body.email;
        // const password = await middlewares.GenerateID(8);
        const password = req.body.password || '';
        const name = req.body.first_name || '';
        const surname = req.body.last_name || '';

        let UIN = 'U' + await middlewares.GeneratenumberID(4);

        if (mobile_number != '' && password != '') {
            if (mobile_number != null && mobile_number != '') {
                // if (email != null && email != '') {
                //     await knex('Users')
                //         .where({ 'mobile_number': mobile_number, 'email': email })
                //         .then(async (bothdata) => {
                //             if (bothdata.length > 0) {
                //                 // both already provide
                //                 res.status(400).send(await middlewares.responseMiddleWares('mobile_email_already_available', false, undefined, 400));
                //             } else {
                                await knex('Users')
                                    .where({ 'mobile_number': mobile_number })
                                    .then(async (mobiledata) => {
                                        if (mobiledata.length > 0) {
                                            // mobile already provide
                                            res.status(400).send(await middlewares.responseMiddleWares('mobile_already_available', false, undefined, 400));
                                        } else {
                                            // await knex('Users')
                                            //     .where({ 'email': email })
                                            //     .then(async (emaildata) => {
                                            //         if (emaildata.length > 0) {
                                            //             // email already provide
                                            //             res.status(400).send(await middlewares.responseMiddleWares('email_already_available', false, undefined, 400));
                                            //         } else {
                                                        let data = {
                                                            user_type: 3,
                                                            name: name,
                                                            surname: surname,
                                                            mobile_number: mobile_number,
                                                            email: email,
                                                            password: sha256(password).toString(),
                                                            region: req.body.region,
                                                            active_date: new Date(),
                                                            created_at: new Date(),
                                                            updated_at: new Date()
                                                        }

                                                        await knex('Users')
                                                            .insert(data)
                                                            .then(async (adduser) => {
                                                                if (adduser > 0) {

                                                                    const token = jwt.sign({ id: adduser[0], user_type: 3 }, config.secret_key);

                                                                    let user_v = {
                                                                        user_device_token: token,
                                                                        fcm_token: req.body.fcm_token,
                                                                        user_device_id: req.body.user_device_id,
                                                                        is_login: 'Y',
                                                                        updated_by: adduser[0],
                                                                        updated_at: new Date(),
                                                                    }

                                                                    user_v['user_id'] = adduser[0];
                                                                    user_v['created_by'] = adduser[0];
                                                                    user_v['created_at'] = new Date()

                                                                    await knex('user_device')
                                                                        .insert(user_v)
                                                                        .then(async (devicedata) => {

                                                                        })

                                                                    await knex('users_information')
                                                                        .where({ UIN: UIN })
                                                                        .then(async (moduledetails) => {
                                                                            if (moduledetails.length > 0) {
                                                                                UIN = 'U' + await middlewares.GeneratenumberID(4);

                                                                                let moduledata = {
                                                                                    user_id: adduser[0],
                                                                                    UIN: UIN,
                                                                                    login_access: 'Y',
                                                                                    created_by: adduser[0],
                                                                                    created_at: new Date(),
                                                                                    updated_by: adduser[0],
                                                                                    updated_at: new Date(),
                                                                                }

                                                                                await knex('users_information')
                                                                                    .insert(moduledata)
                                                                                    .then(async (addmoduledata) => {

                                                                                    })

                                                                                // const full_name = req.body.name + ' ' + req.body.surname;
                                                                                // const subject = await middlewares.config_details('user_registered_subject');

                                                                                // let mail_data = {
                                                                                //     mail_to: req.body.email,
                                                                                //     body_tempate: 'user_registed_mail_template',
                                                                                //     subject: subject,
                                                                                //     name: full_name,
                                                                                //     otp: '',
                                                                                //     mobile_number: mobile_number,
                                                                                //     password: password,
                                                                                //     distributor_code: '',
                                                                                //     user_type: 3
                                                                                // }

                                                                                // await smtpmail.sendemail(mail_data);

                                                                                let smsdata = {
                                                                                    message: 'Your KASIKILI account registed successfully.',
                                                                                    mobile_number: mobile_number
                                                                                }

                                                                                await smsnotify.sendsmsnotify(smsdata);

                                                                                let userDetails = {
                                                                                    user_id: adduser[0],
                                                                                    accesstoken: token,
                                                                                    user_device_id: req.body.user_device_id,
                                                                                    first_name: name,
                                                                                    last_name: surname,
                                                                                    mobile_number: req.body.mobile_number,
                                                                                    region: req.body.region,
                                                                                    balance: 0
                                                                                }

                                                                                res.status(200).send(await middlewares.responseMiddleWares('user_registered', true, userDetails, 200));
                                                                            } else {
                                                                                let moduledata = {
                                                                                    user_id: adduser[0],
                                                                                    UIN: UIN,
                                                                                    login_access: 'Y',
                                                                                    created_by: adduser[0],
                                                                                    created_at: new Date(),
                                                                                    updated_by: adduser[0],
                                                                                    updated_at: new Date(),
                                                                                }

                                                                                await knex('users_information')
                                                                                    .insert(moduledata)
                                                                                    .then(async (addmoduledata) => {

                                                                                    })


                                                                                // const subject = await middlewares.config_details('user_registered_subject');
                                                                                // const full_name = req.body.first_name + ' ' + req.body.last_name;

                                                                                // let mail_data = {
                                                                                //     mail_to: req.body.email,
                                                                                //     body_tempate: 'user_registed_mail_template',
                                                                                //     subject: subject,
                                                                                //     name: full_name,
                                                                                //     otp: '',
                                                                                //     mobile_number: mobile_number,
                                                                                //     password: password,
                                                                                //     distributor_code: '',
                                                                                //     user_type: 3
                                                                                // }

                                                                                // await smtpmail.sendemail(mail_data);

                                                                                let smsdata = {
                                                                                    message: 'Your KASIKILI account registed successfully.',
                                                                                    mobile_number: mobile_number
                                                                                }

                                                                                await smsnotify.sendsmsnotify(smsdata);

                                                                                let userDetails = {
                                                                                    user_id: adduser[0],
                                                                                    accesstoken: token,
                                                                                    user_device_id: req.body.user_device_id,
                                                                                    first_name: name,
                                                                                    last_name: surname,
                                                                                    mobile_number: req.body.mobile_number,
                                                                                    region: req.body.region,
                                                                                    balance: 0
                                                                                }

                                                                                res.status(200).send(await middlewares.responseMiddleWares('user_registered', true, userDetails, 200));
                                                                            }
                                                                        })
                                                                } else {
                                                                    // distributor not create
                                                                    res.status(400).send(await middlewares.responseMiddleWares('user_not_registered', false, undefined, 400));
                                                                }
                                                            })
                                                //     }
                                                // })
                                        }
                                    })
                //             }
                //         })
                // } else {
                //     // email not provide
                //     res.status(400).send(await middlewares.responseMiddleWares('email_not_provide', false, undefined, 400));
                // }
            } else {
                // mobile not provide
                res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
            }
        } else {
            // both not provide
            res.status(400).send(await middlewares.responseMiddleWares('password_mobile_not_provide', false, undefined, 400));
        }
    } catch (err) {
        console.log("registed", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.checkmobilenumber = async (req, res) => {
    try {
        const mobile_number = req.body.mobile_number || '';

        if (mobile_number != null && mobile_number != '') {
            await knex('Users')
                .where({ 'mobile_number': mobile_number })
                .then(async (mobiledata) => {
                    if (mobiledata.length > 0) {
                        // mobile already provide
                        res.status(400).send(await middlewares.responseMiddleWares('mobile_already_available', false, undefined, 400));
                    } else {
                        res.status(200).send(await middlewares.responseMiddleWares('mobile_vaild', true, undefined, 200));
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
        }
    } catch (err) {
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.checkemail = async (req, res) => {
    try {
        const email = req.body.email || '';

        if (email != null && email != '') {
            await knex('Users')
                .where({ 'email': email })
                .then(async (mobiledata) => {
                    if (mobiledata.length > 0) {
                        // mobile already provide
                        res.status(400).send(await middlewares.responseMiddleWares('email_already_available', false, undefined, 400));
                    } else {
                        res.status(200).send(await middlewares.responseMiddleWares('email_vaild', true, undefined, 200));
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('email_not_provide', false, undefined, 400));
        }
    } catch (err) {
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// User & Admin forgetpassword
module.exports.forgetPassword = async (req, res) => {
    try {
        const user_type = req.body.user_type || '';

        if (user_type === 'Admin') {
            const password = await middlewares.GenerateID(8);

            await knex('Users')
                .select(knex.raw(`CONCAT(name, ' ', surname) as 'full_name'`), 'email', 'user_type')
                .where({ email: req.body.email })
                .then(async (checkPass) => {
                    if (checkPass.length > 0) {
                        checkPass = checkPass[0];
                        await knex('Users')
                            .update({ password: sha256(password).toString() })
                            .where({ email: req.body.email })
                            .then(async (newPass) => {
                                if (newPass > 0) {
                                    const subject = await checkPass.user_type == 1 ? middlewares.config_details('admin_password_forget_subject') : middlewares.config_details('staff_password_forget_subject');
                                    const body_tempate = checkPass.user_type == 1 ? 'admin_password_forget_template' : 'staff_password_forget_template';

                                    let mail_data = {
                                        mail_to: req.body.email,
                                        body_tempate: body_tempate,
                                        subject: subject,
                                        name: checkPass.full_name,
                                        otp: '',
                                        mobile_number: checkPass.email,
                                        password: password,
                                        distributor_code: '',
                                        user_type: 1
                                    }

                                    await smtpmail.sendemail(mail_data);

                                    res.status(200).send(await middlewares.responseMiddleWares('password_update', true, undefined, 200));
                                }
                                else {
                                    res.status(400).send(await middlewares.responseMiddleWares('password_notupdate', false, undefined, 400));
                                }
                            })
                    }
                    else {
                        res.status(400).send(await middlewares.responseMiddleWares('current_password_notmatch', false, undefined, 400));
                    }
                })
        } else {
            await knex('Users')
                .where({ mobile_number: req.body.mobile_number })
                .then(async (checkPass) => {
                    if (checkPass.length > 0) {
                        checkPass = checkPass[0];
                        await knex('Users')
                            .update({ password: sha256(req.body.new_password).toString() })
                            .where({ mobile_number: req.body.mobile_number })
                            .then(async (newPass) => {
                                if (newPass > 0) {
                                    res.status(200).send(await middlewares.responseMiddleWares('password_update', true, undefined, 200));
                                }
                                else {
                                    res.status(400).send(await middlewares.responseMiddleWares('password_notupdate', false, undefined, 400));
                                }
                            })
                    }
                    else {
                        res.status(400).send(await middlewares.responseMiddleWares('current_password_notmatch', false, undefined, 400));
                    }
                })
        }
    }
    catch (err) {
        console.log("forgetPassword", err);
        res.send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.changePassword = async (req, res) => {
    try {
        await knex('Users')
            .where({ id: req.user.id, password: sha256(req.body.oldpassword).toString() })
            .then(async (checkPass) => {
                if (checkPass.length > 0) {
                    if (req.body.oldpassword == req.body.newpassword) {
                        res.status(400).send(await middlewares.responseMiddleWares('old_password_same', false, undefined, 400));
                    } else {
                        checkPass = checkPass[0];
                        await knex('Users')
                            .update({ password: sha256(req.body.newpassword).toString() })
                            .where({ id: req.user.id })
                            .then(async (newPass) => {
                                if (newPass > 0) {

                                    // const subject = await middlewares.config_details('password_changed_subject');
                                    // const template = await middlewares.config_details('password_changed_template');

                                    // let mail_data = {
                                    //     mail_to: checkPass.email,
                                    //     subject: subject,
                                    //     template: template
                                    // }

                                    // await smtpmail.sendemail(mail_data);

                                    res.status(200).send(await middlewares.responseMiddleWares('password_update', true, undefined, 200));
                                }
                                else {
                                    res.status(400).send(await middlewares.responseMiddleWares('password_notupdate', false, undefined, 400));
                                }
                            })
                    }
                }
                else {
                    res.status(400).send(await middlewares.responseMiddleWares('current_password_notmatch', false, undefined, 400));
                }
            })
    }
    catch (err) {
        console.log("changePassword", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.updateUserFCMToken = async (req, res) => {
    try {
        if (req.user.user_type == 3 || req.user.user_type == 2) {
            await knex('user_device')
                .where({ 'user_id': req.user.id })
                .then(async (devicedata) => {
                    if (devicedata.length > 0) {
                        await knex('user_device')
                            .update({
                                fcm_token: req.body.fcmToken,
                                user_device_id: req.body.user_device_id,
                                device_date: req.body.device_date,
                                updated_by: req.user.id,
                                updated_at: new Date(),
                            }).where({ user_id: req.user.id })
                            .then(async (updatedetails) => {
                                if (updatedetails > 0) {
                                    res.status(200).send(await middlewares.responseMiddleWares("fcmtoken_update", true, undefined, 200))
                                }
                                else {
                                    res.status(400).send(await middlewares.responseMiddleWares("fcm_not_update", false, undefined, 400))
                                }
                            })
                    } else {
                        // no user found
                        res.status(400).send(await middlewares.responseMiddleWares('fcm_not_update', false, undefined, 400));
                    }
                })
        } else {
            await knex('user_device')
                .where({ 'user_id': req.user.id })
                .then(async (devicedata) => {
                    if (devicedata.length > 0) {
                        const updatedata = {
                            fcm_token: req.body.fcmToken,
                            updated_by: req.user.id,
                            updated_at: new Date(),
                        }
                        await knex('user_device')
                            .update(updatedata)
                            .where({ 'user_id': req.user.id })
                            .then(async (updatedetails) => {
                                if (updatedetails > 0) {
                                    res.status(200).send(await middlewares.responseMiddleWares("fcmtoken_update", true, undefined, 200))
                                }
                                else {
                                    res.status(400).send(await middlewares.responseMiddleWares("fcm_not_update", false, undefined, 400))
                                }
                            })
                    } else {
                        // not user found
                        res.status(400).send(await middlewares.responseMiddleWares('fcm_not_update', false, undefined, 400));
                    }
                })
        }
    }
    catch (error) {
        console.log("error", error);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.logout = async (req, res) => {
    try {
        if (req.user.user_type == 1) {
            await knex('user_device')
                .update({ 'fcm_token': '', 'user_device_token': '', 'is_login': 'N' })
                .where({ 'user_id': req.user.id })
                .then(async (updatedetails) => {
                    if (updatedetails > 0) {
                        res.status(200).send(await middlewares.responseMiddleWares("logout", true, undefined, 200))
                    } else {
                        res.status(400).send(await middlewares.responseMiddleWares("logout_fail", false, undefined, 400))
                    }
                })
        } else {
            if (req.user.id) {
                await knex('user_device')
                    .update({ 'fcm_token': '', 'user_device_token': '', 'is_login': 'N' })
                    .where({ 'user_id': req.user.id })
                    .then(async (updatedetails) => {
                        if (updatedetails > 0) {
                            res.status(200).send(await middlewares.responseMiddleWares("logout", true, undefined, 200))
                        } else {
                            res.status(400).send(await middlewares.responseMiddleWares("logout_fail", false, undefined, 400))
                        }
                    })
            } else {
                await knex('user_device')
                    .where({
                        'user_id': req.body.id,
                    })
                    .update({ 'fcm_token': '', 'user_device_token': '', 'is_login': 'N' })
                    .then(async (updatedetails) => {
                        if (updatedetails > 0) {
                            res.status(200).send(await middlewares.responseMiddleWares("logout", true, undefined, 200))
                        } else {
                            res.status(400).send(await middlewares.responseMiddleWares("logout_fail", false, undefined, 400))
                        }
                    })
            }
        }
    }
    catch (error) {
        console.log("errr", error);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}