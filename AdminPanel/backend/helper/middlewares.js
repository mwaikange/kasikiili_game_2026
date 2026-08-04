const knex = require('knex')(require('../helper/db'));

const config = require('./config');
const jwt = require('jsonwebtoken');
const cryptojs = require('./crypto');

const routeMiddleWares = (req, res, next) => {
    const bearerHeader = req.headers['authorization'];

    if (typeof bearerHeader !== 'undefined') {
        const token = bearerHeader.split(' ')[1];
        if (token !== 'null') {
            return jwt.verify(token, config.secret_key, async (err, userData) => {
                if (err) {
                    res.status(401).json({ success: false, message: "User is not authenticated", success_code: 401 });
                }
                else {
                        await knex('Users')
                            .where({
                                id: userData.id,
                                is_active: 'Y',
                                is_delete: 'N'
                            })
                            .then(async (userdetails) => {
                                if (userdetails.length > 0) {
                                    await knex('user_device')
                                        .where({ user_id: userData.id, user_device_token: token, is_login: 'Y' })
                                        .then(async (userdetails) => {
                                            if (userdetails.length > 0) {
                                                req.user = userData;
                                                next();
                                            } else {
                                                res.status(401).json({ success: false, message: "User is not authenticated", success_code: 401 });
                                            }
                                        })
                                }
                                else {
                                    res.status(401).json({ success: false, message: "User is not authenticated", success_code: 401 });
                                }
                            });
                }
            })
        } else {
            req.user;
            next();
        }
    } else {
        res.status(401).json({ success: false, message: "token missing" })
    }
}

const routeDecryptMiddleWares = (req, res, next) => {
    req.body = cryptojs.decrypt(req.body.info);
    next()
}

const responseMiddleWares = async (key, status, data, code) => {
    /* console.log(key, status, data, code, type); */
    return await knex('site_configuration')
        .where({
            'config_key': key
        })
        .then(async (configDetails) => {
            let return_obj = await {
                message: configDetails[0].config_value,
                success: status,
                success_code: code
            }
            if (configDetails.length <= 0) { return_obj['message'] = "config key not get pls contact admin" };

            if (data) { return_obj['data'] = await cryptojs.encrypt(data) }
            return await return_obj;
        });
}

const config_details = async (key) => {
    return await knex('site_configuration')
        .select('config_value')
        .where({ 'config_key': key })
        .then(async (configDetails) => {

            configDetails = await configDetails[0].config_value

            return await configDetails;
        });
}

const GenerateID = (length) => {
    let id = "";
    const characters = 'AaBb0CcDd1EeFf2GgHh3IiJj4KkLl5MmNn6OoPp7QqRr8SsTt9UuVvWwXxYyZz';
    const charactersLength = characters.length;

    for (let i = 0; i < length; i++) {
        id += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return id;
}

const GenerateDistributorID = (length) => {
    let id = "";
    const characters = 'AB0CD1EF2GH3IJ4KL5MN6OP7QR8ST9UVWXYZ';
    const charactersLength = characters.length;

    for (let i = 0; i < length; i++) {
        id += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return id;
}

const GeneratenumberID = (length) => {
    let id = "";
    const characters = '0123456789';
    const charactersLength = characters.length;

    for (let i = 0; i < length; i++) {
        id += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return id;
}

module.exports = { routeMiddleWares, responseMiddleWares, GenerateID, GenerateDistributorID, GeneratenumberID, routeDecryptMiddleWares, config_details }