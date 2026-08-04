const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const excel = require("exceljs");

module.exports.getalldistributor = async (req, res) => {
    try {
        const filtervalue = req.body.filtervalue || '';
        const filtervalue2 = req.body.filtervalue2 || '';

        let exceldata = [];

        await knex('Users as u')
            .select('u.id', 'u.created_at as reg_date', 'u.mobile_number', knex.raw("CASE WHEN u.is_active = 'Y' THEN 'ENABLED' ELSE 'DISABLED' END AS status"), 'u.region', 'info.distributor_code', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 2 })
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
                            .where('u.is_active', status)
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

                    for(let dat of ditributordata) {
                        let data = {
                            'REG DATE': dat.reg_date,
                            'MOBILE NO.': dat.mobile_number,
                            'STATUS': dat.status,
                            'REGION': dat.region,
                            'BALANCE': dat.balance,
                            'DISTRIBUTOR CODE': dat.distributor_code
                        }

                        exceldata.push(data);
                    }

                    const data = {
                        ditributordata: ditributordata,
                        exceldata: exceldata
                    }

                    res.status(200).send(await middlewares.responseMiddleWares('distributor_data', true, data, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('distributor_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getditributordata = async (req, res) => {
    try {
        const distributor_id = req.params.id;

        await knex('Users as u')
            .select('u.id', 'u.mobile_number', 'u.region', 'u.email', knex.raw(`CONCAT(u.name, ' ', u.surname) as 'full_name'`), 'info.distributor_code', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 2, 'u.id': distributor_id })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('distributor_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('distributor_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getditributortranscation = async (req, res) => {
    try {
        const distributor_id = req.params.id;

        await knex('Transcation Management as t')
            .select('t.transation_id', 't.amount', 't.tra_status', 't.trascantion_date', knex.raw("CASE WHEN from_usertype_id > 1 THEN u.mobile_number ELSE 'Admin' END AS recipient"))
            .leftJoin('Users as u', 't.to_trascation_id', 'u.id')
            .where({'t.req_flag': 'N'})
            .where({ 't.from_trascation': distributor_id })
            .orWhere({ 't.to_trascation_id': distributor_id })
            .limit(15)
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

module.exports.downloaddistdata = async (req, res) => {
    try {
        let tutorials = [];
        const date = new Date();
        const filename = "distributor_" + date.getDate() + "_" + (date.getMonth() + 1) + ".xlsx";

        const filtervalue = req.body.filtervalue || '';
        const filtervalue2 = req.body.filtervalue2 || '';

        await knex('Users as u')
            .select('u.id', 'u.created_at as reg_date', 'u.mobile_number', knex.raw("CASE WHEN u.is_active = 'Y' THEN 'ENABLED' ELSE 'DISABLED' END AS status"), 'u.region', 'info.distributor_code', 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 2 })
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
                            .where('u.is_active', status)
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

                        console.log("date", initialdate, finaldate);
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
                    ditributordata.forEach((obj) => {
                        tutorials.push({
                            reg_date: obj.reg_date,
                            mobile_number: obj.mobile_number,
                            status: obj.status,
                            region: obj.region,
                            distributor_code: obj.distributor_code,
                            balance: obj.balance
                        });
                    });

                    let workbook = new excel.Workbook();
                    let worksheet = workbook.addWorksheet("Distributor");

                    worksheet.columns = [
                        { header: "Reg. Date", key: "reg_date", width: 15 },
                        { header: "Mobile Number", key: "mobile_number", width: 15 },
                        { header: "Status", key: "status", width: 10 },
                        { header: "Region", key: "region", width: 15 },
                        { header: "Distributor Code", key: "distributor_code", width: 15 },
                        { header: "Balance", key: "balance", width: 10 },
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
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('distributor_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// distributor list for user
module.exports.getdistributorslist = async (req, res) => {
    try {
        await knex('Users as u')
            .select('u.id', 'u.mobile_number', 'u.region', knex.raw(`CONCAT(u.name, ' ', u.surname) as "name"`), 'info.credit_balance as balance')
            .leftJoin('users_information as info', 'u.id', 'info.user_id')
            .where({ 'u.user_type': 2, 'u.is_active': 'Y', 'u.is_delete': 'N' })
            .then(async (ditributordata) => {
                if (ditributordata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('distributor_data', true, ditributordata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('distributor_not_data', false, [], 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}