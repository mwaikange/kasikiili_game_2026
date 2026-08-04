const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const smtpmail = require('../helper/emailnotification');
const smsnotify = require('../helper/smsnotification');

//stored mobileno for user
module.exports.otpSend = async (req, res) => {
    try {
        if (req.body.mobile_number) {
            await knex('Users')
                .select('id', knex.raw(`CONCAT(name, ' ', surname) as 'full_name'`), 'mobile_number', 'email', 'is_active', 'is_delete', 'user_type')
                .where({ 'mobile_number': req.body.mobile_number })
                .then(async (rdata) => {
                    if (rdata.length > 0) {
                        rdata = rdata[0]
                        let random = Math.floor(100000 + Math.random() * 900000);
                        // let random = 123456;
                        const now = new Date();
                        const expiration_time = await AddMinutesToDate(now, 1);

                        let number = req.body.mobile_number;

                        if (rdata.is_active == 'N') {
                            res.status(400).send(await middlewares.responseMiddleWares("account_deactivate", false, undefined, 400));
                            return false;
                        }

                        if (rdata.is_delete == 'Y') {
                            res.status(400).send(await middlewares.responseMiddleWares("account_delete", false, undefined, 400));
                            return false;
                        }

                        await knex('otp_master')
                            .where({ 'mobile_number': number })
                            .then(async (otpdetails) => {
                                if (otpdetails.length > 0) {
                                    await knex('otp_master')
                                        .update({
                                            'otp': random,
                                            'email': rdata.email,
                                            'is_verify': 'Y',
                                            'created_by': rdata.id,
                                            'updated_by': rdata.id,
                                            'created_date': new Date(),
                                            'updated_date': new Date(),
                                            'expiration_time': expiration_time
                                        })
                                        .where({ 'mobile_number': number })
                                        .then(async (result) => {
                                            if (result > 0) {

                                                if (rdata.email) {
                                                    const subject = await middlewares.config_details('OTP_send_subject');

                                                    let mail_data = {
                                                        mail_to: rdata.email,
                                                        body_tempate: 'send_otp_template',
                                                        subject: subject,
                                                        name: rdata.full_name,
                                                        otp: random,
                                                        mobile_number: '',
                                                        password: '',
                                                        distributor_code: '',
                                                        user_type: rdata.user_type
                                                    }

                                                    await smtpmail.sendemail(mail_data);
                                                }

                                                let smsdata = {
                                                    message: `Your Kasikili OTP: ${random}`,
                                                    mobile_number: number
                                                }

                                                await smsnotify.sendsmsnotify(smsdata);

                                                res.status(200).send(await middlewares.responseMiddleWares('otp_send_success', true, undefined, 200));
                                            }
                                        })
                                }
                                else {
                                    await knex('otp_master')
                                        .insert({
                                            'otp': random,
                                            'mobile_number': number,
                                            'email': rdata.email,
                                            'is_verify': 'Y',
                                            'created_by': rdata.id,
                                            'updated_by': rdata.id,
                                            'created_date': new Date(),
                                            'updated_date': new Date(),
                                            'expiration_time': expiration_time
                                        })
                                        .then(async (result) => {
                                            if (result > 0) {

                                                if (rdata.email) {
                                                    const subject = await middlewares.config_details('OTP_send_subject');

                                                    let mail_data = {
                                                        mail_to: rdata.email,
                                                        body_tempate: 'send_otp_template',
                                                        subject: subject,
                                                        name: rdata.full_name,
                                                        otp: random,
                                                        mobile_number: '',
                                                        password: '',
                                                        distributor_code: '',
                                                        user_type: rdata.user_type
                                                    }

                                                    await smtpmail.sendemail(mail_data);
                                                }

                                                let smsdata = {
                                                    message: `Your Kasikili OTP: ${random}`,
                                                    mobile_number: number
                                                }

                                                await smsnotify.sendsmsnotify(smsdata);

                                                res.status(200).send(await middlewares.responseMiddleWares('otp_send_success', true, undefined, 200));
                                            }
                                        })
                                }
                            })
                    }
                    else {
                        res.status(400).send(await middlewares.responseMiddleWares('otp_mobile_not_match', false, undefined, 400))
                    }
                })
        } else {

        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400))
    }
}

/* unstored mobile no pending */
module.exports.sendRegistrationOtp = async (req, res) => {
    try {
        let random = Math.floor(100000 + Math.random() * 900000);
        let number = req.body.mobile_number;

        const now = new Date();
        const expiration_time = await AddMinutesToDate(now, 15);

        if (number != '' && number != undefined && number != null) {
            await knex('Users')
                .where({ 'mobile_number': req.body.mobile_number })
                .then(async (registerDetails) => {
                    if (registerDetails.length > 0) {
                        res.send(await middlewares.responseMiddleWares("dup_mobile_entry", false, undefined, 400))
                    }
                    else {
                        await knex('otp_master')
                            .where({ 'mobile_number': number })
                            .then(async (otpdetails) => {
                                if (otpdetails.length > 0) {
                                    await knex('otp_master')
                                        .update({
                                            'otp': random,
                                            'is_verify': 'N',
                                            'created_date': new Date(),
                                            'updated_date': new Date(),
                                            'expiration_time': expiration_time
                                        })
                                        .where({ 'mobile_number': number })
                                        .then(async (result) => {
                                            if (result > 0) {
                                                let smsdata = {
                                                    message: `Your Kasikili OTP: ${random}`,
                                                    mobile_number: number
                                                }

                                                await smsnotify.sendsmsnotify(smsdata);

                                                res.status(200).send(await middlewares.responseMiddleWares('otp_send_success', true, undefined, 200));
                                            }
                                        })
                                }
                                else {
                                    await knex('otp_master')
                                        .insert({
                                            'otp': random,
                                            'mobile_number': number,
                                            'is_verify': 'N',
                                            'created_date': new Date(),
                                            'updated_date': new Date(),
                                            'expiration_time': expiration_time
                                        })
                                        .then(async (result) => {
                                            if (result > 0) {
                                                let smsdata = {
                                                    message: `Your Kasikili OTP: ${random}`,
                                                    mobile_number: number
                                                }

                                                await smsnotify.sendsmsnotify(smsdata);

                                                res.status(200).send(await middlewares.responseMiddleWares('otp_send_success', true, undefined, 200));
                                            }
                                        })
                                }
                            })
                    }
                });
        } else {
            // mobile not provide
            res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
        }
    }
    catch (err) { console.log("err", err); res.send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400)) }
}

module.exports.verifyOtp = async (req, res) => {
    try {
        let currentdate = new Date();

        if (req.body.mobile_number) {
            var number = req.body.mobile_number;
            await knex('otp_master')
                .where({ 'mobile_number': number, 'otp': req.body.otp })
                .limit(1)
                .then(async (rdata) => {
                    if (rdata.length > 0) {
                        if (dates.compare(rdata[0].expiration_time, currentdate) == 1) {
                            await knex('otp_master')
                                .del()
                                .where({ 'otp': req.body.otp }).then(async (result) => {
                                    if (result > 0) {
                                        res.status(200).send(await middlewares.responseMiddleWares('otp_match', true, undefined, 200));
                                    }
                                })
                        } else {
                            await knex('otp_master')
                                .del()
                                .where({ 'otp': req.body.otp }).then(async (result) => {
                                    if (result > 0) {
                                        res.status(400).send(await middlewares.responseMiddleWares('otp_notexpired', false, undefined, 400));
                                    }
                                })
                        }
                    }
                    else {
                        res.status(400).send(await middlewares.responseMiddleWares('otp_notmatch', false, undefined, 400))
                    }
                })
                .catch(async (err) => {
                    res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400))
                })
        } else {
            // mobile not provide
            res.status(400).send(await middlewares.responseMiddleWares('mobile_not_provide', false, undefined, 400));
        }
    } catch (err) {
        console.log("err-1", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400))
    }
}

function AddMinutesToDate(date, minutes) {
    return new Date(date.getTime() + minutes * 60000);
}

var dates = {
    convert: function (d) {
        // Converts the date in d to a date-object. The input can be:
        //   a date object: returned without modification
        //  an array      : Interpreted as [year,month,day]. NOTE: month is 0-11.
        //   a number     : Interpreted as number of milliseconds
        //                  since 1 Jan 1970 (a timestamp)
        //   a string     : Any format supported by the javascript engine, like
        //                  "YYYY/MM/DD", "MM/DD/YYYY", "Jan 31 2009" etc.
        //  an object     : Interpreted as an object with year, month and date
        //                  attributes.  **NOTE** month is 0-11.
        return (
            d.constructor === Date ? d :
                d.constructor === Array ? new Date(d[0], d[1], d[2]) :
                    d.constructor === Number ? new Date(d) :
                        d.constructor === String ? new Date(d) :
                            typeof d === "object" ? new Date(d.year, d.month, d.date) :
                                NaN
        );
    },
    compare: function (a, b) {
        // Compare two dates (could be of any type supported by the convert
        // function above) and returns:
        //  -1 : if a < b
        //   0 : if a = b
        //   1 : if a > b
        // NaN : if a or b is an illegal date
        return (
            isFinite(a = this.convert(a).valueOf()) &&
                isFinite(b = this.convert(b).valueOf()) ?
                (a > b) - (a < b) :
                NaN
        );
    },
    inRange: function (d, start, end) {
        // Checks if date in d is between dates in start and end.
        // Returns a boolean or NaN:
        //    true  : if d is between start and end (inclusive)
        //    false : if d is before start or after end
        //    NaN   : if one or more of the dates is illegal.
        return (
            isFinite(d = this.convert(d).valueOf()) &&
                isFinite(start = this.convert(start).valueOf()) &&
                isFinite(end = this.convert(end).valueOf()) ?
                start <= d && d <= end :
                NaN
        );
    }
}