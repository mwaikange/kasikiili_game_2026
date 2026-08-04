const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');

// send credit from distributor to user
module.exports.sendcredit = async (req, res) => {
    try {
        const amount = parseInt(req.body.amount);
        const transation_id = 'txt_' + await middlewares.GenerateID(8);
        const mobile_number = req.body.mobile_number;

        if (mobile_number != null && mobile_number != '') {
            await knex('users_information as info')
                .select('u.id', 'info.credit_balance', 'u.email', 'u.mobile_number')
                .leftJoin('Users as u', 'info.user_id', 'u.id')
                .where({ 'u.mobile_number': mobile_number })
                .then(async (infodata) => {
                    if (infodata.length > 0) {
                        infodata = infodata[0];

                        await knex('users_information')
                            .select('credit_balance')
                            .where({ 'user_id': req.user.id })
                            .then(async (disdata) => {
                                disdata = disdata[0];
                                let dis_balance = disdata.credit_balance - amount;

                                if (dis_balance > 0) {
                                    let total_balance = amount + infodata.credit_balance;

                                    let txt_data = {
                                        User_type_id: 3,
                                        transation_id: transation_id,
                                        amount: amount,
                                        tra_status: "Transfer",
                                        from_usertype_id: 2,
                                        from_trascation: req.user.id,
                                        to_trascation_id: infodata.id,
                                        trascantion_date: new Date(),
                                        created_by: req.user.id,
                                        created_at: new Date(),
                                        updated_by: req.user.id,
                                        updated_at: new Date()
                                    }

                                    await knex('Transcation Management')
                                        .insert(txt_data)
                                        .then(async (tdata) => {
                                            if (tdata > 0) {
                                                await knex('users_information')
                                                    .update({ 'credit_balance': dis_balance, 'updated_by': req.user.id, 'updated_at': new Date() })
                                                    .where({ 'user_id': req.user.id })
                                                    .then(async (disdata) => {

                                                    })

                                                await knex('users_information')
                                                    .update({ 'credit_balance': total_balance, 'updated_by': req.user.id, 'updated_at': new Date() })
                                                    .where({ 'user_id': infodata.id })
                                                    .then(async (disdata) => {

                                                    })

                                                res.status(200).send(await middlewares.responseMiddleWares('credit_send', true, undefined, 200));
                                            } else {
                                                res.status(400).send(await middlewares.responseMiddleWares('credit_not_send', false, undefined, 400));
                                            }
                                        })
                                } else {
                                    res.status(400).send(await middlewares.responseMiddleWares('insufficient_credit', false, undefined, 400));
                                }
                            })
                    } else {
                        res.status(400).send(await middlewares.responseMiddleWares('user_not_found', false, undefined, 400));
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.cashout = async (req, res) => {
    try {
        const amount = parseInt(req.body.amount);
        const req_id = 'req_' + await middlewares.GenerateID(6);

        let req_data = {
            User_type_id: 1,
            amount: amount,
            tra_status: "Req CASH OUT",
            from_usertype_id: 3,
            from_trascation: req.user.id,
            trascantion_date: new Date(),
            req_flag: 'Y',
            req_id: req_id,
            created_by: req.user.id,
            created_at: new Date(),
            updated_by: req.user.id,
            updated_at: new Date()
        }

        if (amount >= 50 & amount <= 5000) {
            await knex('Transcation Management')
                .where({ 'from_trascation': req.user.id, 'req_flag': 'Y' })
                .orderBy('id', 'desc')
                .limit(1)
                .then(async (tdata) => {
                    if (tdata.length > 0) {
                        res.status(400).send(await middlewares.responseMiddleWares('user_already_req', false, undefined, 400));
                    } else {
                        await knex('users_information as info')
                            .select('u.id', 'info.credit_balance', 'u.email', 'u.mobile_number')
                            .leftJoin('Users as u', 'info.user_id', 'u.id')
                            .where({ 'u.id': req.user.id })
                            .then(async (infodata) => {
                                if (infodata.length > 0) {
                                    infodata = infodata[0];

                                    if (parseInt(infodata.credit_balance) >= amount) {
                                        let total_balance = parseInt(infodata.credit_balance) - amount;

                                        await knex('Transcation Management')
                                            .insert(req_data)
                                            .then(async (tdata) => {
                                                if (tdata > 0) {
                                                    await knex('users_information')
                                                        .update({ 'credit_balance': total_balance, 'updated_by': req.user.id, 'updated_at': new Date() })
                                                        .where({ 'user_id': req.user.id })
                                                        .then(async (udata) => {

                                                        })

                                                    await knex('Users')
                                                        .update({ 'req_flag': 'Y', 'updated_by': req.user.id, 'updated_at': new Date() })
                                                        .where({ 'id': req.user.id })
                                                        .then(async (udata) => {

                                                        })
                                                    res.status(200).send(await middlewares.responseMiddleWares('user_req_added', true, undefined, 200));
                                                } else {
                                                    res.status(400).send(await middlewares.responseMiddleWares('user_req_not_added', false, undefined, 400));
                                                }
                                            })
                                    } else {
                                        res.status(400).send(await middlewares.responseMiddleWares('insufficient_credit', false, undefined, 400));
                                    }
                                } else {
                                    res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
                                }
                            })
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('cashout_50_req', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.acceptcashout = async (req, res) => {
    try {
        const req_id = req.body.req_id;
        const transation_id = 'txt_' + await middlewares.GenerateID(8);

        await knex('Transcation Management')
            .where({ 'req_id': req_id })
            .then(async (reqdata) => {
                if (reqdata.length > 0) {
                    reqdata = reqdata[0];

                    await knex('users_information')
                        .select('credit_balance')
                        .where({ 'user_id': reqdata.from_trascation })
                        .then(async (udata) => {
                            // let total_amount = parseInt(udata[0].credit_balance) - reqdata.amount;
                            let total_amount = parseInt(reqdata.amount);

                            if (total_amount >= 0) {

                                let txt_data = {
                                    transation_id: transation_id,
                                    tra_status: "CASH OUT",
                                    trascantion_date: new Date(),
                                    req_flag: 'N',
                                    req_id: null,
                                    updated_by: req.user.id,
                                    updated_at: new Date()
                                }

                                await knex('Transcation Management')
                                    .update(txt_data)
                                    .where({ 'id': reqdata.id })
                                    .then(async (tdata) => {
                                        if (tdata > 0) {
                                            // await knex('users_information')
                                            //     .update({ 'credit_balance': total_amount, updated_by: req.user.id, updated_at: new Date() })
                                            //     .where({ 'user_id': reqdata.from_trascation })
                                            //     .then(async (disdata) => {

                                            //     })

                                            await knex('Users')
                                                .update({ 'req_flag': 'N', 'updated_by': req.user.id, 'updated_at': new Date() })
                                                .where({ 'id': reqdata.from_trascation })
                                                .then(async (udata) => {

                                                })

                                            res.status(200).send(await middlewares.responseMiddleWares('cashout_accept', true, undefined, 200));
                                        } else {
                                            res.status(400).send(await middlewares.responseMiddleWares('cashout_not_accept', false, undefined, 400));
                                        }
                                    })
                            } else {
                                res.status(400).send(await middlewares.responseMiddleWares('insufficient_credit', false, undefined, 400));
                            }
                        })
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('req_cashout_not_found', false, undefined, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// user transcation
module.exports.getusertranscation = async (req, res) => {
    try {
        await knex('Transcation Management as t')
            .select('t.transation_id', 't.amount', knex.raw("CASE WHEN t.tra_status = 'Transfer' THEN 'CASH IN' ELSE 'SYSTEM' END AS recipient"), 't.trascantion_date')
            .leftJoin('Users as u', 't.to_trascation_id', 'u.id')
            .leftJoin('Users as dis', 't.from_trascation', 'u.id')
            .leftJoin('users_information as disinfo', 'disinfo.user_id', 'dis.id')
            .where({ 't.req_flag': 'N' })
            .where({ 't.from_trascation': req.user.id })
            .orWhere({ 't.to_trascation_id': req.user.id })
            .limit(5)
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

// Distributor transcation
module.exports.getdistributortranscation = async (req, res) => {
    try {
        await knex('Transcation Management as t')
            .select('t.transation_id', 't.amount', 't.tra_status', 't.trascantion_date', knex.raw("CASE WHEN from_usertype_id > 1 THEN u.mobile_number ELSE 'Admin' END AS recipient"))
            // .select('t.transation_id', 't.amount', 't.tra_status', 't.trascantion_date')
            .leftJoin('Users as u', 't.to_trascation_id', 'u.id')
            .leftJoin('Users as dis', 't.from_trascation', 'u.id')
            .leftJoin('users_information as disinfo', 'disinfo.user_id', 'dis.id')
            .where({ 't.req_flag': 'N' })
            .where({ 't.from_trascation': req.user.id })
            .orWhere({ 't.to_trascation_id': req.user.id })
            .limit(5)
            .orderBy('t.id', 'desc')
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('distributor_trans_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('distributor_trans_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getupdatedistributorbalance = async (req, res) => {
    try {
        await knex('Users as u')
            .select('disinfo.credit_balance')
            .leftJoin('users_information as disinfo', 'disinfo.user_id', 'u.id')
            .where({ 'u.id': req.user.id })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    let data = {
                        avialablebalance: ditributordata[0].credit_balance
                    }
                    res.status(200).send(await middlewares.responseMiddleWares('distributor_updated_balance', true, data, 200));
                } else {
                    let data = {
                        avialablebalance: 0
                    }
                    res.status(400).send(await middlewares.responseMiddleWares('distributor_updated_balance_not', false, data, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.updateuserbalance = async (req, res) => {
    try {
        const amount = parseInt(req.body.amount) || 0;
        const status = req.body.status;
        let total_credit, total_balance;

        const transation_id = 'txt_' + await middlewares.GenerateID(8);

        await knex('Users as u')
            .select('disinfo.credit_balance', 'disinfo.winning_balance')
            .leftJoin('users_information as disinfo', 'disinfo.user_id', 'u.id')
            .where({ 'u.id': req.user.id })
            .then(async (userdata) => {
                if (userdata.length > 0) {
                    userdata = userdata[0];
                    total_credit = amount;
                    // if (status == 'Won' || status == 'won') {
                    //     total_credit = parseInt(userdata.credit_balance) + amount;
                    //     total_balance = parseInt(userdata.winning_balance) + amount;
                    // } else {
                    //     total_credit = parseInt(userdata.credit_balance) - amount;
                    //     total_balance = parseInt(userdata.winning_balance) - amount;
                    // }

                    if (total_credit > 0) {

                        // total_balance = total_balance <= 0 ? 0 : total_balance;
                        // let txt_data = {
                        //     User_type_id: 3,
                        //     transation_id: transation_id,
                        //     amount: amount,
                        //     tra_status: status,
                        //     from_usertype_id: 3,
                        //     to_trascation_id: req.user.id,
                        //     trascantion_date: new Date(),
                        //     created_by: req.user.id,
                        //     created_at: new Date(),
                        //     updated_by: req.user.id,
                        //     updated_at: new Date()
                        // }

                        // await knex('Transcation Management')
                        //     .insert(txt_data)
                        //     .then(async (tdata) => {
                        //         if (tdata > 0) {
                                        await knex('users_information')
                                            .update({ 'credit_balance': total_credit, 'updated_by': req.user.id, 'updated_at': new Date() })
                                            .where({ 'user_id': req.user.id })
                                            .then(async (disdata) => {

                                                let data = {
                                                    balance: total_credit,
                                                }

                                                res.status(200).send(await middlewares.responseMiddleWares('user_balance_update', true, data, 200));
                                            })
                        //     } else {
                        //         res.status(400).send(await middlewares.responseMiddleWares('user_balance_not_update', false, undefined, 400));
                        //     }
                        // })
                    } else {
                        await knex('users_information')
                        .update({ 'credit_balance': 0, 'updated_by': req.user.id, 'updated_at': new Date() })
                        .where({ 'user_id': req.user.id })
                        .then(async (disdata) => {

                            let data = {
                                balance: 0
                            }

                            res.status(200).send(await middlewares.responseMiddleWares('user_balance_update', true, data, 200));
                        })
                    }
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_not_found', false, undefined, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getuserbalance = async (req, res) => {
    try {

        await knex('Users as u')
            .select('disinfo.credit_balance', 'disinfo.winning_balance')
            .leftJoin('users_information as disinfo', 'disinfo.user_id', 'u.id')
            .where({ 'u.id': req.user.id })
            .then(async (userdata) => {
                if (userdata.length > 0) {
                    userdata = userdata[0];
                    let data = {
                        balance: userdata.credit_balance,
                        winning_balance: userdata.winning_balance
                    }

                    res.status(200).send(await middlewares.responseMiddleWares('user_balance_update', true, data, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('user_balance_not_update', false, undefined, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}