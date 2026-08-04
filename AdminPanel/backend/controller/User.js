const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const sha256 = require('crypto-js/sha256');
const excel = require("exceljs");
const smtpmail = require('../helper/emailnotification');
const smsnotify = require('../helper/smsnotification');

const cryptojs = require('../helper/crypto');

module.exports.createusertype = async (req, res) => {
    try {
        let data = {
            name: req.body.name,
            created_by: req.user.id,
            created_at: new Date(),
            updated_by: req.user.id,
            updated_at: new Date(),
        }

        await knex('user_type')
            .where({ 'name': req.body.name })
            .then(async (addata) => {
                if (addata.length > 0) {
                    res.status(400).send(await middlewares.responseMiddleWares('usertype_already_available', false, undefined, 400))
                } else {
                    await knex('user_type')
                        .insert(data)
                        .then(async (adddata) => {
                            if (adddata > 0) {
                                res.status(200).send(await middlewares.responseMiddleWares('usertype_added', true, undefined, 200));
                            } else {
                                res.status(400).send(await middlewares.responseMiddleWares('usertype_not_add', false, undefined, 400));
                            }
                        })
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// Create Staff
module.exports.createadmin = async (req, res) => {
    try {
        const mobile_number = req.body.mobile_number || '';
        const email = req.body.email || '';
        const password = await middlewares.GenerateID(8);
        // const password = "12345678";

        if (mobile_number != '' && email != '') {
            if (mobile_number != null && mobile_number != '') {
                if (email != null && email != '') {
                    await knex('Users')
                        .where({ 'mobile_number': mobile_number, 'email': email })
                        .then(async (bothdata) => {
                            if (bothdata.length > 0) {
                                // both already provide
                                res.status(400).send(await middlewares.responseMiddleWares('mobile_email_already_available', false, undefined, 400));
                            } else {
                                await knex('Users')
                                    .where({ 'mobile_number': mobile_number })
                                    .then(async (mobiledata) => {
                                        if (mobiledata.length > 0) {
                                            // mobile already provide
                                            res.status(400).send(await middlewares.responseMiddleWares('mobile_already_available', false, undefined, 400));
                                        } else {
                                            await knex('Users')
                                                .where({ 'email': email })
                                                .then(async (emaildata) => {
                                                    if (emaildata.length > 0) {
                                                        // email already provide
                                                        res.status(400).send(await middlewares.responseMiddleWares('email_already_available', false, undefined, 400));
                                                    } else {
                                                        let data = {
                                                            user_type: 4,
                                                            name: req.body.name,
                                                            surname: req.body.surname,
                                                            mobile_number: mobile_number,
                                                            email: email,
                                                            password: sha256(password).toString(),
                                                            created_by: req.user.id,
                                                            created_at: new Date(),
                                                            updated_by: req.user.id,
                                                            updated_at: new Date(),
                                                            active_date: new Date(),
                                                        }

                                                        await knex('Users')
                                                            .insert(data)
                                                            .then(async (adduser) => {
                                                                if (adduser > 0) {
                                                                    const module_data = req.body.module_data;

                                                                    for (let module of module_data) {
                                                                        let moduledata = {
                                                                            user_id: adduser[0],
                                                                            module_id: module,
                                                                            created_by: req.user.id,
                                                                            created_at: new Date(),
                                                                            updated_by: req.user.id,
                                                                            updated_at: new Date(),
                                                                        }

                                                                        await knex('module_access_management')
                                                                            .insert(moduledata)
                                                                            .then(async (addmoduledata) => {

                                                                            })
                                                                    }

                                                                    // const subject = await middlewares.config_details('Admin_created_subject');
                                                                    const subject = await middlewares.config_details('Staff_created_subject');
                                                                    const full_name = req.body.name + ' ' + req.body.surname;

                                                                    let mail_data = {
                                                                        mail_to: req.body.email,
                                                                        body_tempate: 'staff_created_mail_template',
                                                                        subject: subject,
                                                                        name: full_name,
                                                                        otp: '',
                                                                        mobile_number: email,
                                                                        password: password,
                                                                        distributor_code: '',
                                                                        user_type: 1
                                                                    }

                                                                    await smtpmail.sendemail(mail_data);

                                                                    let smsdata = {
                                                                        message: 'Your staff account created successfully.\nLogin details sent in your registed mail',
                                                                        mobile_number: mobile_number
                                                                    }

                                                                    await smsnotify.sendsmsnotify(smsdata);

                                                                    res.status(200).send(await middlewares.responseMiddleWares('staff_created', true, undefined, 200));
                                                                } else {
                                                                    // admin not create
                                                                    res.status(400).send(await middlewares.responseMiddleWares('staff_not_create', false, undefined, 400));
                                                                }
                                                            })
                                                    }
                                                })
                                        }
                                    })
                            }
                        })
                } else {
                    // email not provide
                    res.status(400).send(await middlewares.responseMiddleWares('email_not_provide', false, undefined, 400));
                }
            } else {
                // mobile not provide
                res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
            }
        } else {
            // both not provide
            res.status(400).send(await middlewares.responseMiddleWares('email_mobile_not_provide', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// create Distributor
module.exports.creatdistributo = async (req, res) => {
    try {
        const mobile_number = req.body.mobile_number || '';
        const email = req.body.email || '';
        const password = await middlewares.GenerateID(8);
        // const password = "12345678";

        let distributor_code = 'd_' + await middlewares.GenerateDistributorID(4);

        if (mobile_number != '' && email != '') {
            if (mobile_number != null && mobile_number != '') {
                if (email != null && email != '') {
                    await knex('Users')
                        .where({ 'mobile_number': mobile_number, 'email': email })
                        .then(async (bothdata) => {
                            if (bothdata.length > 0) {
                                // both already provide
                                res.status(400).send(await middlewares.responseMiddleWares('mobile_email_already_available', false, undefined, 400));
                            } else {
                                await knex('Users')
                                    .where({ 'mobile_number': mobile_number })
                                    .then(async (mobiledata) => {
                                        if (mobiledata.length > 0) {
                                            // mobile already provide
                                            res.status(400).send(await middlewares.responseMiddleWares('mobile_already_available', false, undefined, 400));
                                        } else {
                                            await knex('Users')
                                                .where({ 'email': email })
                                                .then(async (emaildata) => {
                                                    if (emaildata.length > 0) {
                                                        // email already provide
                                                        res.status(400).send(await middlewares.responseMiddleWares('email_already_available', false, undefined, 400));
                                                    } else {
                                                        let data = {
                                                            user_type: 2,
                                                            name: req.body.name,
                                                            surname: req.body.surname,
                                                            mobile_number: mobile_number,
                                                            email: email,
                                                            password: sha256(password).toString(),
                                                            region: req.body.region,
                                                            created_by: req.user.id,
                                                            created_at: new Date(),
                                                            updated_by: req.user.id,
                                                            updated_at: new Date(),
                                                            active_date: new Date(),
                                                        }

                                                        await knex('Users')
                                                            .insert(data)
                                                            .then(async (adduser) => {
                                                                if (adduser > 0) {
                                                                    await knex('users_information')
                                                                        .where({ distributor_code: distributor_code })
                                                                        .then(async (moduledetails) => {
                                                                            if (moduledetails.length > 0) {
                                                                                distributor_code = 'd_' + await middlewares.GenerateDistributorID(4);

                                                                                let moduledata = {
                                                                                    user_id: adduser[0],
                                                                                    distributor_code: distributor_code,
                                                                                    login_access: 'Y',
                                                                                    created_by: req.user.id,
                                                                                    created_at: new Date(),
                                                                                    updated_by: req.user.id,
                                                                                    updated_at: new Date(),
                                                                                }

                                                                                await knex('users_information')
                                                                                    .insert(moduledata)
                                                                                    .then(async (addmoduledata) => {

                                                                                    })


                                                                                const subject = await middlewares.config_details('distributor_created_subject');
                                                                                const full_name = req.body.name + ' ' + req.body.surname;

                                                                                let mail_data = {
                                                                                    mail_to: req.body.email,
                                                                                    body_tempate: 'distributor_created_mail_template',
                                                                                    subject: subject,
                                                                                    name: full_name,
                                                                                    otp: '',
                                                                                    mobile_number: mobile_number,
                                                                                    password: password,
                                                                                    distributor_code: distributor_code,
                                                                                    user_type: 2
                                                                                }

                                                                                await smtpmail.sendemail(mail_data);

                                                                                let smsdata = {
                                                                                    message: 'Your distributor account created successfully.\nLogin details sent in your registed mail',
                                                                                    mobile_number: mobile_number
                                                                                }

                                                                                await smsnotify.sendsmsnotify(smsdata);

                                                                                res.status(200).send(await middlewares.responseMiddleWares('distributor_created', true, undefined, 200));
                                                                            } else {
                                                                                let moduledata = {
                                                                                    user_id: adduser[0],
                                                                                    distributor_code: distributor_code,
                                                                                    login_access: 'Y',
                                                                                    created_by: req.user.id,
                                                                                    created_at: new Date(),
                                                                                    updated_by: req.user.id,
                                                                                    updated_at: new Date(),
                                                                                }

                                                                                await knex('users_information')
                                                                                    .insert(moduledata)
                                                                                    .then(async (addmoduledata) => {

                                                                                    })

                                                                                const subject = await middlewares.config_details('distributor_created_subject');
                                                                                const full_name = req.body.name + ' ' + req.body.surname;

                                                                                let mail_data = {
                                                                                    mail_to: req.body.email,
                                                                                    body_tempate: 'distributor_created_mail_template',
                                                                                    subject: subject,
                                                                                    name: full_name,
                                                                                    otp: '',
                                                                                    mobile_number: mobile_number,
                                                                                    password: password,
                                                                                    distributor_code: distributor_code,
                                                                                    user_type: 2
                                                                                }

                                                                                await smtpmail.sendemail(mail_data);

                                                                                let smsdata = {
                                                                                    message: 'Your distributor account created successfully.\nLogin details sent in your registed mail',
                                                                                    mobile_number: mobile_number
                                                                                }

                                                                                await smsnotify.sendsmsnotify(smsdata);

                                                                                res.status(200).send(await middlewares.responseMiddleWares('distributor_created', true, undefined, 200));
                                                                            }
                                                                        })
                                                                } else {
                                                                    // distributor not create
                                                                    res.status(400).send(await middlewares.responseMiddleWares('distributor_not_create', false, undefined, 400));
                                                                }
                                                            })
                                                    }
                                                })
                                        }
                                    })
                            }
                        })
                } else {
                    // email not provide
                    res.status(400).send(await middlewares.responseMiddleWares('email_not_provide', false, undefined, 400));
                }
            } else {
                // mobile not provide
                res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
            }
        } else {
            // both not provide
            res.status(400).send(await middlewares.responseMiddleWares('email_mobile_not_provide', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// Reset Distributor password
module.exports.resetdistributorpassword = async (req, res) => {
    try {
        const distributor_code = req.body.distributor_code || '';
        const password = await middlewares.GenerateID(8);
        // const password = "12345678";

        if (distributor_code != null && distributor_code != '') {
            await knex('users_information as info')
                .leftJoin('Users as u', 'info.user_id', 'u.id')
                .where({ 'info.distributor_code': distributor_code })
                .then(async (distributordata) => {
                    if (distributordata.length > 0) {
                        const user_id = distributordata[0].user_id;

                        await knex('Users')
                            .update({ 'password': sha256(password).toString(), updated_by: req.user.id, updated_at: new Date() })
                            .where({ 'id': user_id })
                            .then(async (updatedata) => {
                                if (updatedata > 0) {
                                    const subject = await middlewares.config_details('distributor_reset_pass_subject');

                                    const full_name = distributordata[0].name + ' ' + distributordata[0].surname;

                                    let mail_data = {
                                        mail_to: distributordata[0].email,
                                        body_tempate: 'distributor_reset_password',
                                        subject: subject,
                                        name: full_name,
                                        otp: '',
                                        mobile_number: distributordata[0].mobile_number,
                                        password: password,
                                        distributor_code: distributordata[0].distributor_code,
                                        user_type: 2
                                    }

                                    await smtpmail.sendemail(mail_data);

                                    let smsdata = {
                                        message: 'Your account password reset successfully.\nLogin details sent in your registed mail',
                                        mobile_number: distributordata[0].mobile_number
                                    }

                                    await smsnotify.sendsmsnotify(smsdata);

                                    res.status(200).send(await middlewares.responseMiddleWares('distributor_pass_reset', true, undefined, 200));
                                } else {
                                    res.status(400).send(await middlewares.responseMiddleWares('distributor_pass_not_reset', false, undefined, 400));
                                }
                            })
                    } else {
                        // not
                        res.status(400).send(await middlewares.responseMiddleWares('distributor_not_found', false, undefined, 400));
                    }
                })
        } else {
            // not provide
            res.status(400).send(await middlewares.responseMiddleWares('distributor_not_provide', false, undefined, 400));
        }
    } catch (error) {
        console.log("errr", error);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// active/deactive Distributor
module.exports.changestatusdistributor = async (req, res) => {
    try {
        let config_key = "";
        let config_key_not = "";
        const mobile_number = req.body.mobile_number;
        const status = req.body.status || 'Y';

        const password = await middlewares.GenerateID(8);
        // const password = "12345678";

        let updatedata = {};

        if (status == 'Y') {
            config_key = "distributor_actived";
            config_key_not = "distributor_not_actived";
            updatedata['is_active'] = 'Y';
            updatedata['active_date'] = new Date();
            updatedata['updated_by'] = req.user.id;
            updatedata['updated_at'] = new Date();
        }

        if (status == 'N') {
            config_key = "distributor_deactived";
            config_key_not = "distributor_not_deactived";
            updatedata['is_active'] = 'N';
            updatedata['active_date'] = null;
            updatedata['updated_by'] = req.user.id;
            updatedata['updated_at'] = new Date();
        }

        await knex('Users')
            .update(updatedata)
            .where({ 'mobile_number': mobile_number, 'user_type': 2 })
            .then(async (updata) => {
                if (updata > 0) {
                    await knex('Users as u')
                        .select('u.email', 'u.mobile_number', knex.raw(`CONCAT(u.name, ' ', u.surname) as 'full_name'`), 'info.distributor_code')
                        .leftJoin('users_information as info', 'info.user_id', 'u.id')
                        .where({ 'u.mobile_number': mobile_number, 'u.user_type': 2 })
                        .then(async (udata) => {
                            udata = udata[0];

                            const subject = await status == 'Y' ? middlewares.config_details('distributor_status_enable_subject') : middlewares.config_details('distributor_status_disable_subject');

                            const template = status == 'Y' ? 'distributor_activated_template' : 'distributor_deactivate_template';

                            const full_name = udata.full_name;

                            let mail_data = {
                                mail_to: udata.email,
                                body_tempate: template,
                                subject: subject,
                                name: full_name,
                                otp: '',
                                mobile_number: udata.mobile_number,
                                password: password,
                                distributor_code: udata.distributor_code,
                                user_type: 2
                            }

                            await smtpmail.sendemail(mail_data);

                            res.status(200).send(await middlewares.responseMiddleWares(config_key, true, undefined, 200));
                        })
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares(config_key_not, false, undefined, 400))
                }
            })

    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// active/deactive User
module.exports.changestatususer = async (req, res) => {
    try {
        let config_key = "";
        let config_key_not = "";
        const mobile_number = req.body.mobile_number;
        const status = req.body.status || 'Y';
        const password = await middlewares.GenerateID(8);
        // const password = "12345678";

        let updatedata = {};

        if (status == 'Y') {
            config_key = "user_actived";
            config_key_not = "user_not_actived";
            updatedata['is_active'] = 'Y';
            updatedata['active_date'] = new Date();
            updatedata['updated_by'] = req.user.id;
            updatedata['updated_at'] = new Date();
        }

        if (status == 'N') {
            config_key = "user_deactived";
            config_key_not = "user_not_deactived";
            updatedata['is_active'] = 'N';
            updatedata['active_date'] = null;
            updatedata['updated_by'] = req.user.id;
            updatedata['updated_at'] = new Date();
        }

        await knex('Users')
            .update(updatedata)
            .where({ 'mobile_number': mobile_number, 'user_type': 3 })
            .then(async (updata) => {
                if (updata > 0) {
                    await knex('Users')
                        .select('email', 'mobile_number', knex.raw(`CONCAT(name, ' ', surname) as 'full_name'`))
                        .where({ 'mobile_number': mobile_number, 'user_type': 3 })
                        .then(async (udata) => {
                            udata = udata[0];

                            const subject = await status == 'Y' ? middlewares.config_details('user_status_enable_subject') : middlewares.config_details('user_status_disable_subject');
                            const template = status == 'Y' ? 'user_activated_template' : 'user_deactivate_template';

                            const full_name = udata.full_name;

                            let mail_data = {
                                mail_to: udata.email,
                                body_tempate: template,
                                subject: subject,
                                name: full_name,
                                otp: '',
                                mobile_number: udata.mobile_number,
                                password: password,
                                distributor_code: '',
                                user_type: 3
                            }

                            await smtpmail.sendemail(mail_data);

                            res.status(200).send(await middlewares.responseMiddleWares(config_key, true, undefined, 200));
                        })
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares(config_key_not, false, undefined, 400))
                }
            })

    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// getallusers data
module.exports.getallusers = async (req, res) => {
    try {
        const filtervalue = req.body.filtervalue || '';
        const filtervalue2 = req.body.filtervalue2 || '';

        let exceldata = [];

        await knex('Users as u')
            .select('u.id', 'u.created_at as reg_date', 'u.mobile_number', knex.raw("CASE WHEN u.is_active = 'Y' THEN 'ENABLED' ELSE 'DISABLED' END AS status"), 'u.region', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            // .leftOuterJoin('Transcation Management as tm', 'u.id', 'tm.from_trascation')
            // .groupBy('tm.from_trascation')
            .where({ 'u.user_type': 3 })
            .modify(function (queryBuilder) {
                if (filtervalue) {
                    if (filtervalue.filter == "Mobile no") {
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.mobile_number', 'like', `%${filtervalue.search}%`)
                    } else if (filtervalue.filter == "Status") {
                        let status = 'Y';
                        if (filtervalue.search == 'ENABLED' || filtervalue.search == 'Enabled' || filtervalue.search == 'enabled') {
                            status = 'Y';
                        } else {
                            status = 'N';
                        }
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where({ 'u.is_active': status })
                    } else if (filtervalue.filter == "Region") {
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.region', 'like', `%${filtervalue.search}%`)
                    } else if (filtervalue.filter == "List From Highest Balance") { // LOHB -- Highest balance
                        queryBuilder
                            .orderBy('info.credit_balance', 'desc')
                    } else if (filtervalue.filter == "List From Lowest Balance") {  // LOLB -- Lowest balance
                        queryBuilder
                            .orderBy('info.credit_balance', 'asc')
                    } else if (filtervalue.filter == "Request Status") {
                        let status = 'Y';
                        if (filtervalue.search == 'Cashout Request' || filtervalue.search == 'Enabled' || filtervalue.search == 'enabled') {
                            status = 'Y';
                        } else {
                            status = 'N';
                        }

                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where({ 'u.req_flag': status })

                    } else if (filtervalue.filter == "daterange") {
                        let date = req.body.search;
                        date = date.split(',');

                        let initialdate = new Date(date[0]);
                        initialdate = initialdate.getFullYear() + '-' + (initialdate.getMonth() + 1) + '-' + initialdate.getDate()
                        let finaldate = new Date(date[1]);
                        finaldate = finaldate.getFullYear() + '-' + (finaldate.getMonth() + 1) + '-' + finaldate.getDate()

                        // console.log("date", initialdate, finaldate);
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.created_at', '>=', initialdate)
                            .where('u.created_at', '<', finaldate)
                    } else {
                        queryBuilder.orderBy('u.id', 'desc');
                    }
                }
            })
            .modify(function (queryBuilder) {
                if (filtervalue2) {
                    if (filtervalue2.filter == "daterange") {
                        const from = filtervalue2.from || '';
                        const To = filtervalue2.to || '';
                        let initialdate = new Date(from);
                        initialdate = initialdate.getFullYear() + '-' + (initialdate.getMonth() + 1) + '-' + initialdate.getDate()
                        let finaldate = new Date(To);
                        finaldate = finaldate.getFullYear() + '-' + (finaldate.getMonth() + 1) + '-' + finaldate.getDate()

                        // console.log("date", initialdate, finaldate);
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.created_at', '>=', initialdate)
                            .where('u.created_at', '<', finaldate)
                    } else {
                        queryBuilder.orderBy('u.id', 'desc');
                    }
                }
            })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    for (let dat of ditributordata) {
                        dat['checked'] = false;
                        await knex('Transcation Management')
                            .select('req_flag', 'req_id', 'amount')
                            .where({ 'from_trascation': dat.id })
                            .limit(1)
                            .orderBy('id', 'desc')
                            .then(async (tdata) => {
                                if (tdata.length > 0) {
                                    tdata = tdata[0];
                                    if (tdata.req_flag == 'Y') {
                                        dat['req_amount'] = tdata.amount;
                                        dat['req_id'] = tdata.req_id;
                                        dat['req_flag'] = 'Y';
                                    } else {
                                        dat['req_amount'] = '-';
                                        dat['req_id'] = '-';
                                        dat['req_flag'] = 'N';
                                    }
                                } else {
                                    dat['req_amount'] = '-';
                                    dat['req_id'] = '-';
                                    dat['req_flag'] = 'N';
                                }
                            })
                    }

                    for (let dat of ditributordata) {
                        let data = {
                            'REG DATE': dat.reg_date,
                            'MOBILE NO.': dat.mobile_number,
                            'STATUS': dat.status,
                            'REGION': dat.region,
                            'BALANCE': dat.balance,
                            'REQUEST': dat.req_id,
                            'AMOUNT': dat.req_amount
                        }

                        exceldata.push(data);
                    }

                    const data = {
                        userdata: ditributordata,
                        exceldata: exceldata
                    }

                    res.status(200).send(await middlewares.responseMiddleWares('user_data', true, data, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getallusersdownload = async (req, res) => {
    try {
        let tutorials = [];
        const date = new Date();
        const filename = "user_" + date.getDate() + "_" + (date.getMonth() + 1) + ".xlsx";

        const filtervalue = req.body.filtervalue || '';
        const filtervalue2 = req.body.filtervalue2 || '';

        await knex('Users as u')
            .select('u.id', 'u.created_at as reg_date', 'u.mobile_number', knex.raw("CASE WHEN u.is_active = 'Y' THEN 'ENABLED' ELSE 'DISABLED' END AS status"), 'u.region', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 3 })
            .modify(function (queryBuilder) {
                if (filtervalue) {
                    if (filtervalue.filter == "mobile_number") {
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.mobile_number', 'like', `%${filtervalue.search}%`)
                    } else if (filtervalue.filter == "status") {
                        let status = 'Y';
                        if (filtervalue.search == 'ENABLED' || filtervalue.search == 'Enabled' || filtervalue.search == 'enabled') {
                            status = 'Y';
                        } else {
                            status = 'N';
                        }
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.is_active', status)
                    } else if (filtervalue.filter == "region") {
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.region', 'like', `%${filtervalue.search}%`)
                    } else if (filtervalue.filter == "LOHB") { // LOHB -- Highest balance
                        queryBuilder
                            .orderBy('info.credit_balance', 'desc')
                    } else if (filtervalue.filter == "LOLB") {  // LOLB -- Lowest balance
                        queryBuilder
                            .orderBy('info.credit_balance', 'asc')
                    } else if (filtervalue.filter == "daterange") {
                        let date = req.body.search;
                        date = date.split(',');

                        let initialdate = new Date(date[0]);
                        initialdate = initialdate.getFullYear() + '-' + (initialdate.getMonth() + 1) + '-' + initialdate.getDate()
                        let finaldate = new Date(date[1]);
                        finaldate = finaldate.getFullYear() + '-' + (finaldate.getMonth() + 1) + '-' + finaldate.getDate()

                        // console.log("date", initialdate, finaldate);
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.created_at', '>=', initialdate)
                            .where('u.created_at', '<', finaldate)
                    } else {
                        queryBuilder.orderBy('u.id', 'desc');
                    }
                }
            })
            .modify(function (queryBuilder) {
                if (filtervalue2) {
                    if (filtervalue2.filter == "daterange") {
                        const from = filtervalue2.from || '';
                        const To = filtervalue2.to || '';
                        let initialdate = new Date(from);
                        initialdate = initialdate.getFullYear() + '-' + (initialdate.getMonth() + 1) + '-' + initialdate.getDate()
                        let finaldate = new Date(To);
                        finaldate = finaldate.getFullYear() + '-' + (finaldate.getMonth() + 1) + '-' + finaldate.getDate()

                        // console.log("date", initialdate, finaldate);
                        queryBuilder
                            .orderBy('u.id', 'desc')
                            .where('u.created_at', '>=', initialdate)
                            .where('u.created_at', '<', finaldate)
                    } else {
                        queryBuilder.orderBy('u.id', 'desc');
                    }
                }
            })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    for (let dat of ditributordata) {
                        await knex('Transcation Management')
                            .select('req_flag', 'req_id', 'amount')
                            .where({ 'from_trascation': dat.id })
                            .limit(1)
                            .then(async (tdata) => {
                                tdata = tdata[0];
                                console.log("tdata", tdata);
                                if (tdata.req_flag == 'Y') {
                                    dat['req_amount'] = tdata.amount;
                                    dat['req_id'] = tdata.req_id;
                                    dat['req_flag'] = 'Y';
                                } else {
                                    dat['req_amount'] = '-';
                                    dat['req_id'] = '-';
                                    dat['req_flag'] = 'N';
                                }
                            })
                    }

                    ditributordata.forEach((obj) => {
                        tutorials.push({
                            reg_date: obj.reg_date,
                            mobile_number: obj.mobile_number,
                            status: obj.status,
                            region: obj.region,
                            balance: obj.balance,
                            req_id: obj.req_id,
                            req_amount: obj.req_amount,
                        });
                    });

                    let workbook = new excel.Workbook();
                    let worksheet = workbook.addWorksheet("Distributor");

                    worksheet.columns = [
                        { header: "Reg. Date", key: "reg_date", width: 15 },
                        { header: "Mobile Number", key: "mobile_number", width: 15 },
                        { header: "Status", key: "status", width: 10 },
                        { header: "Region", key: "region", width: 15 },
                        { header: "Balance", key: "balance", width: 10 },
                        { header: "Request ID", key: "req_id", width: 15 },
                        { header: "Request Amount", key: "req_amount", width: 15 },
                    ];

                    // Add Array Rows
                    worksheet.addRows(tutorials);

                    res.setHeader(
                        "Content-Type",
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    );
                    res.setHeader(
                        "Content-Disposition",
                        "attachment; filename=" + filename
                    );

                    return workbook.xlsx.write(res).then(function () {
                        res.status(200).end();
                    });

                    res.status(200).send(await middlewares.responseMiddleWares('user_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getuserdata = async (req, res) => {
    try {
        const user_id = req.params.id;

        await knex('Users as u')
            .select('u.id', 'u.mobile_number', 'u.region', 'u.email', knex.raw(`CONCAT(u.name, ' ', u.surname) as 'full_name'`), 'info.UIN', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 3, 'u.id': user_id })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('user_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getusertranscation = async (req, res) => {
    try {
        const user_id = req.params.id;

        await knex('Transcation Management as t')
            .select('t.transation_id', 't.amount', knex.raw("CASE WHEN t.tra_status = 'Transfer' THEN 'CASH IN' ELSE t.tra_status END AS status"), 't.trascantion_date', knex.raw("CASE WHEN from_usertype_id = 2 THEN info.distributor_code ELSE 'SYSTEM' END AS distributor"))
            .leftJoin('Users as u', 't.from_trascation', 'u.id')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 't.req_flag': 'N' })
            .where({ 't.from_trascation': user_id })
            .orWhere({ 't.to_trascation_id': user_id })
            .limit(15)
            .orderBy('t.id', 'desc')
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('user_trans_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_trans_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// Profile
module.exports.getprofiledata = async (req, res) => {
    try {
        await knex('Users as u')
            .select('u.id', 'u.mobile_number', 'u.region', 'u.email', knex.raw(`CONCAT(u.name, ' ', u.surname) as 'full_name'`), 'u.name', 'u.surname', 'info.UIN', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.id': req.user.id })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('user_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// Awaiting Balance
module.exports.getuserawaitingcashout = async (req, res) => {
    try {
        await knex('Users as u')
            .select('t.amount as awaitingcashout')
            .leftJoin('Transcation Management as t', 'u.id', 't.from_trascation')
            .where({ 't.from_trascation': req.user.id, 't.req_flag': 'Y' })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    let data = {
                        awaitingcashout: ditributordata[0].awaitingcashout
                    }
                    res.status(200).send(await middlewares.responseMiddleWares('awaitingcashout_data', true, data, 200));
                } else {
                    let data = {
                        awaitingcashout: 0
                    }
                    res.status(400).send(await middlewares.responseMiddleWares('awaitingcashout_not_data', false, data, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}