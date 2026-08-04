const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');

const smtpmail = require('../helper/emailnotification');
const cryptojs = require('../helper/crypto');

module.exports.generatebalance = async (req, res) => {
    try {
        let amount = parseInt(req.body.amount);

        let email_arr = [];

        await knex('Total_Amount')
            .select('total_amount')
            .orderBy('id', 'desc')
            .then(async (amtdata) => {
                if (amtdata.length > 0) {
                    amtdata = amtdata[0];
                    let total_amount = amount + parseInt(amtdata.total_amount);

                    let gereratedata = {
                        amount: amount,
                        total_amount: total_amount,
                        status: 'add',
                        created_by: req.user.id,
                        created_at: new Date(),
                        updated_by: req.user.id,
                        updated_at: new Date(),
                    }

                    await knex('Total_Amount')
                        .insert(gereratedata)
                        .then(async (addamount) => {
                            if (addamount > 0) {
                                await knex('Users')
                                    .select('email', 'mobile_number')
                                    .where({ 'user_type': 1 })
                                    .then(async (admindata) => {
                                        if (admindata.length > 0) {
                                            for (let dat of admindata) {
                                                email_arr.push(dat.email);
                                            }

                                            const subject = await middlewares.config_details('credit_generate_subject');
                                            const template = await middlewares.config_details('credit_generate_template');

                                            let mail_to = await email_arr.join(',');

                                            let mail_data = {
                                                mail_to: mail_to,
                                                subject: subject,
                                                template: template
                                            }

                                            // await smtpmail.sendemail(mail_data);
                                        }
                                    })

                                res.status(200).send(await middlewares.responseMiddleWares('amount_generate', true, undefined, 200));
                            } else {
                                res.status(400).send(await middlewares.responseMiddleWares('amount_not_generate', false, undefined, 400));
                            }
                        })
                } else {
                    let gereratedata = {
                        amount: amount,
                        total_amount: amount,
                        status: 'add',
                        created_by: req.user.id,
                        created_at: new Date(),
                        updated_by: req.user.id,
                        updated_at: new Date(),
                    }

                    await knex('Total_Amount')
                        .insert(gereratedata)
                        .then(async (addamount) => {
                            if (addamount > 0) {
                                await knex('Users')
                                    .select('email')
                                    .where({ 'user_type': 1 })
                                    .then(async (admindata) => {
                                        if (admindata.length > 0) {
                                            for (let dat of admindata) {
                                                email_arr.push(dat.email);
                                            }

                                            const subject = await middlewares.config_details('credit_generate_subject');
                                            const template = await middlewares.config_details('credit_generate_template');

                                            let mail_to = await email_arr.join(',');

                                            let mail_data = {
                                                mail_to: mail_to,
                                                subject: subject,
                                                template: template
                                            }

                                            // await smtpmail.sendemail(mail_data);
                                        }
                                    })
                                res.status(200).send(await middlewares.responseMiddleWares('amount_generate', true, undefined, 200));
                            } else {
                                res.status(400).send(await middlewares.responseMiddleWares('amount_not_generate', false, undefined, 400));
                            }
                        })
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// availabel credit
module.exports.getavailablecredit = async (req, res) => {
    try {
        await knex('Total_Amount')
            .select('total_amount')
            .orderBy('id', 'desc')
            .then(async (amtdata) => {
                if (amtdata.length > 0) {
                    let data = {
                        available_credit: parseInt(amtdata[0].total_amount)
                    }

                    res.status(200).send(await middlewares.responseMiddleWares('available_credit', true, data, 200));
                } else {
                    let data = {
                        available_credit: 0
                    }
                    res.status(200).send(await middlewares.responseMiddleWares('available_credit', true, data, 200));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// TOTAL BALANCE - DISTRIBUTORS
module.exports.getalldistributorbalance = async (req, res) => {
    try {
        await knex('Users as u')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 2 })
            .sum('info.credit_balance as total_available')
            .then(async (getcredit) => {
                getcredit = getcredit[0];
                res.status(200).send(await middlewares.responseMiddleWares('total_credit_d', true, getcredit, 200));
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// TOTAL BALANCE - USERS
module.exports.getallusersbalance = async (req, res) => {
    try {
        await knex('Users as u')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 3 })
            .sum('info.credit_balance as total_available')
            .then(async (getcredit) => {
                getcredit = getcredit[0];
                res.status(200).send(await middlewares.responseMiddleWares('total_credit_u', true, getcredit, 200));
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// Transfer to Distributor
module.exports.transferdistri = async (req, res) => {
    try {
        const distributor_code = req.body.distributor_code || '';
        const amount = parseInt(req.body.amount) || 0;
        const transation_id = 'txt_' + await middlewares.GenerateID(8);

        let email_arr = [];

        if (distributor_code != '' && amount != 0) {
            await knex('Total_Amount')
                .select('total_amount')
                .orderBy('id', 'desc')
                .then(async (amtdata) => {
                    if (amtdata.length > 0) {
                        amtdata = amtdata[0];
                        let total_amount = amtdata.total_amount;

                        if (total_amount >= amount) {
                            await knex('users_information as info')
                                .leftJoin('Users as u', 'info.user_id', 'u.id')
                                .select('info.user_id', 'info.credit_balance', 'u.email', 'u.mobile_number')
                                .where({ 'info.distributor_code': distributor_code })
                                .then(async (infodata) => {
                                    if (infodata.length > 0) {
                                        infodata = infodata[0];

                                        let transdata = {
                                            User_type_id: 2,
                                            user_id: infodata.user_id,
                                            transation_id: transation_id,
                                            amount: amount,
                                            tra_status: "TOP UP",
                                            from_usertype_id: 1,
                                            from_trascation: req.user.id,
                                            to_trascation_id: infodata.user_id,
                                            trascantion_date: new Date(),
                                            created_by: req.user.id,
                                            created_at: new Date(),
                                            updated_by: req.user.id,
                                            updated_at: new Date()
                                        }

                                        await knex('Transcation Management')
                                            .insert(transdata)
                                            .then(async (addtras) => {

                                            })

                                        let total_balance = amount + infodata.credit_balance;

                                        await knex('users_information')
                                            .update({ 'credit_balance': total_balance })
                                            .where({ 'distributor_code': distributor_code })
                                            .then(async (infodata) => {

                                            })

                                        let gereratedata = {
                                            amount: amount,
                                            total_amount: total_amount - amount,
                                            status: 'minus',
                                            created_by: req.user.id,
                                            created_at: new Date(),
                                            updated_by: req.user.id,
                                            updated_at: new Date(),
                                        }

                                        await knex('Total_Amount')
                                            .insert(gereratedata)
                                            .then(async (addamount) => {

                                            })

                                        await knex('Users')
                                            .select('email')
                                            .where({ 'user_type': 1 })
                                            .then(async (admindata) => {
                                                if (admindata.length > 0) {
                                                    // send mail to admin
                                                    for (let dat of admindata) {
                                                        email_arr.push(dat.email);
                                                    }

                                                    const subject = await middlewares.config_details('transfer_credit_to_distributor_subject');
                                                    const template = await middlewares.config_details('transfer_credit_to_distributor_template');

                                                    let mail_to = await email_arr.join(',');

                                                    let mail_data = {
                                                        mail_to: mail_to,
                                                        subject: subject,
                                                        template: template
                                                    }

                                                    // await smtpmail.sendemail(mail_data);

                                                    // send mail to distributo
                                                    const subject_dis = await middlewares.config_details('collect_credit_from_admin_subject');
                                                    const template_dis = await middlewares.config_details('collect_credit_from_admin_template');

                                                    let mail_data_dis = {
                                                        mail_to: infodata.email,
                                                        subject: subject_dis,
                                                        template: template_dis
                                                    }

                                                    // await smtpmail.sendemail(mail_data_dis);
                                                }
                                            })
                                        res.status(200).send(await middlewares.responseMiddleWares('amount_transfer_distributor', true, undefined, 200));
                                    } else {
                                        // not found
                                        res.status(400).send(await middlewares.responseMiddleWares('distributor_not_found', false, undefined, 400));
                                    }
                                })
                        } else {
                            res.status(400).send(await middlewares.responseMiddleWares('credit_not_available', false, undefined, 400));
                        }
                    }
                })
        } else {
            // not provide
            res.status(400).send(await middlewares.responseMiddleWares('code_amount_not', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}