const config = require('../helper/config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const jwt = require('jsonwebtoken');
const sha256 = require('crypto-js/sha256');

const handlebars = require('handlebars');
const nodemailer = require('nodemailer');

module.exports.sendemail = async (data) => {
    try {
        const date = new Date();
        const current_year = date.getFullYear();

        let mail_host = await middlewares.config_details('mail_host');
        let mail_port = await parseInt(middlewares.config_details('mail_port'));
        let mail_auth_email = await middlewares.config_details('mail_auth_email');
        let mail_auth_password = await middlewares.config_details('mail_auth_password');
        let from_mail = await middlewares.config_details('from_mail');

        if (data.user_type == 1) {
            // Admin
            let distributor_header_template = await middlewares.config_details('distributor_header_template');
            let main_body_tempate = await middlewares.config_details(data.body_tempate);
            let distributor_footer_template = await middlewares.config_details('distributor_footer_template');

            // mail data
            let subject = await data.subject;
            let mail_to = await data.mail_to;

            const passObje = {
                'name': data.name,
                'otp': data.otp,
                'current_year': current_year,
                'mobile_number': data.mobile_number,
                'password': data.password,
                'distributor_code': data.distributor_code,
                'terms_url': '',
                'whatup_link': ''
            }

            let source = distributor_header_template + main_body_tempate + distributor_footer_template;

            const template = handlebars.compile(source)(passObje);

            const transporter = nodemailer.createTransport({
                host: mail_host,
                port: mail_port,
                secure: false,
                auth: {
                    user: mail_auth_email,
                    pass: mail_auth_password
                }
            });

            const mailOptions = {
                from: from_mail,
                to: mail_to,
                subject: subject,
                html: template,
            };

            transporter.sendMail(mailOptions, function (error, info) {
                if (error) {
                    console.log(error);
                    return false;
                }
                console.log("mail sent successfully")
                return false;
            });
        } else if (data.user_type == 2) {
            // Distributor
            let distributor_header_template = await middlewares.config_details('distributor_header_template');
            let main_body_tempate = await middlewares.config_details(data.body_tempate);
            let distributor_body_template = await middlewares.config_details('distributor_body_template');
            let distributor_footer_template = await middlewares.config_details('distributor_footer_template');

            let distributor_terms_url = await middlewares.config_details('distributor_terms_url');
            let distributor_whatapp_contact = await middlewares.config_details('distributor_whatapp_contact');

            // mail data
            let subject = await data.subject;
            let mail_to = await data.mail_to;

            const passObje = {
                'name': data.name,
                'otp': data.otp,
                'current_year': current_year,
                'mobile_number': data.mobile_number,
                'password': data.password,
                'distributor_code': data.distributor_code,
                'terms_url': distributor_terms_url,
                'whatup_link': distributor_whatapp_contact
            }

            let source = distributor_header_template + main_body_tempate + distributor_body_template + distributor_footer_template;

            const template = handlebars.compile(source)(passObje);

            const transporter = nodemailer.createTransport({
                host: mail_host,
                port: mail_port,
                secure: false,
                auth: {
                    user: mail_auth_email,
                    pass: mail_auth_password
                }
            });

            const mailOptions = {
                from: from_mail,
                to: mail_to,
                subject: subject,
                html: template,
            };

            transporter.sendMail(mailOptions, function (error, info) {
                if (error) {
                    console.log(error);
                    return false;
                }
                console.log("mail sent successfully")
                return false;
            });
        } else if (data.user_type == 3) {
            // User
            let distributor_header_template = await middlewares.config_details('distributor_header_template');
            let main_body_tempate = await middlewares.config_details(data.body_tempate);
            let distributor_footer_template = await middlewares.config_details('distributor_footer_template');

            // mail data
            let subject = await data.subject;
            let mail_to = await data.mail_to;

            const passObje = {
                'name': data.name,
                'otp': data.otp,
                'current_year': current_year,
                'mobile_number': data.mobile_number,
                'password': data.password,
                'distributor_code': data.distributor_code,
                'terms_url': '',
                'whatup_link': ''
            }

            let source = distributor_header_template + main_body_tempate + distributor_footer_template;

            const template = handlebars.compile(source)(passObje);

            const transporter = nodemailer.createTransport({
                host: mail_host,
                port: mail_port,
                secure: false,
                auth: {
                    user: mail_auth_email,
                    pass: mail_auth_password
                }
            });

            const mailOptions = {
                from: from_mail,
                to: mail_to,
                subject: subject,
                html: template,
            };

            transporter.sendMail(mailOptions, function (error, info) {
                if (error) {
                    console.log(error);
                    return false;
                }
                console.log("mail sent successfully")
                return false;
            });
        }
    } catch (err) {
        console.log(err);
    }
}