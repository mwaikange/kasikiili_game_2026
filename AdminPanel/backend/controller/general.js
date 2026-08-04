const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const jwt = require('jsonwebtoken');
const sha256 = require('crypto-js/sha256');

const handlebars = require('handlebars');
const nodemailer = require('nodemailer');

const { Buffer } = require('buffer');

const cryptojs = require('../helper/crypto');

// TOTAL DISTRIBUTORS
module.exports.getalldistributor = async (req, res) => {
    try {
        await knex('Users as u')
            .where({ 'u.user_type': 2 })
            .count('* as total_distributors')
            .then(async (getcredit) => {
                getcredit = getcredit[0];
                res.status(200).send(await middlewares.responseMiddleWares('total_distributor', true, getcredit, 200));
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// TOTAL USERS
module.exports.getallusers = async (req, res) => {
    try {
        await knex('Users as u')
            .where({ 'u.user_type': 3 })
            .count('* as total_users')
            .then(async (getcredit) => {
                getcredit = getcredit[0];
                res.status(200).send(await middlewares.responseMiddleWares('total_users', true, getcredit, 200));
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

// TOTAL CASH OUT REQ
module.exports.getallcashreq = async (req, res) => {
    try {
        await knex('Transcation Management')
            .where({ 'req_flag': 'Y' })
            .sum('amount as total_cashout_balance')
            .then(async (getcredit) => {
                getcredit = getcredit[0];
                res.status(200).send(await middlewares.responseMiddleWares('total_cash_req', true, getcredit, 200));
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getallcashrequsers = async (req, res) => {
    try {
        await knex('Transcation Management')
            .where({ 'req_flag': 'Y' })
            .count('* as total_cashout_user')
            .then(async (getcredit) => {
                getcredit = getcredit[0];
                res.status(200).send(await middlewares.responseMiddleWares('total_cash_user_req', true, getcredit, 200));
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}