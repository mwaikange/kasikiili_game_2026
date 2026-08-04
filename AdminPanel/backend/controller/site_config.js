const knex = require('knex')(require('../helper/db'));
const config = require('../helper/config');
const middlewares = require('../helper/middlewares');

module.exports.configinsert = async (req, res) => {
    try {
        let data = {
            config_key: req.body.config_key,
            config_value: req.body.config_value,
            created_by: req.user.id,
            created_at: new Date(),
            updated_by: req.user.id,
            updated_at: new Date(),
        }

        await knex('site_configuration')
            .where({ 'config_key': req.body.config_key })
            .then(async (configlist) => {
                if (configlist.length > 0) {
                    res.status(400).send(await middlewares.responseMiddleWares('config_already_available', false, undefined, 400))
                } else {
                    await knex('site_configuration')
                        .insert(data)
                        .then(async (configdata) => {
                            if (configdata > 0) {
                                res.status(200).send(await middlewares.responseMiddleWares('config_added', true, undefined, 200))
                            } else {
                                res.status(400).send(await middlewares.responseMiddleWares('config_not_added', false, undefined, 400))
                            }
                        })
                }
            })
    }
    catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.configupdate = async (req, res) => {
    try {
        const config_key = req.body.config_key;
        if (config_key != null && config_key != '') {
            let data = {
                config_value: req.body.config_value,
                updated_by: req.user.id,
                updated_at: new Date()
            }

            await knex('site_configuration')
                .where({ 'config_key': req.body.config_key })
                .update(data)
                .then(async (configdata) => {
                    if (configdata > 0) {
                        res.status(200).send(await middlewares.responseMiddleWares('config_updated', true, undefined, 200))
                    } else {
                        res.status(400).send(await middlewares.responseMiddleWares('config_not_updated', false, undefined, 400))
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('config_not', false, undefined, 400))
        }
    }
    catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getconfig = async (req, res) => {
    try {
        const config_key = req.body.config_key;
        if (config_key != null && config_key != '') {
            await knex('site_configuration')
                .select('config_value')
                .where({ 'config_key': req.body.config_key })
                .then(async (configdata) => {
                    if (configdata.length > 0) {
                        configdata = configdata[0];
                        res.status(200).send(await middlewares.responseMiddleWares('get_config_data', true, configdata, 200))
                    } else {
                        res.status(400).send(await middlewares.responseMiddleWares('config_not_found', false, undefined, 400))
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('config_not', false, undefined, 400))
        }
    }
    catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getallconfig = async (req, res) => {
    try {
        await knex('site_configuration')
            .select('id', 'config_value', 'config_key')
            .then(async (configdata) => {
                if (configdata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('get_config_data', true, configdata, 200))
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('config_not_found', false, undefined, 400))
                }
            })
    }
    catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}