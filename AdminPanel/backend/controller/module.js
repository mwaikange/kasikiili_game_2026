const knex = require('knex')(require('../helper/db'));
const middlewares = require('../helper/middlewares');
const cryptojs = require('../helper/crypto');

module.exports.addmodule = async (req, res) => {
    try {
        let data = {
            name: req.body.name,
            created_by: req.user.id,
            created_at: new Date(),
            updated_by: req.user.id,
            updated_at: new Date(),
        }

        await knex('module_management')
            .where({ 'name': req.body.name })
            .then(async (addata) => {
                if (addata.length > 0) {
                    res.status(400).send(await middlewares.responseMiddleWares('module_already_available', false, undefined, 400))
                } else {
                    await knex('module_management')
                        .insert(data)
                        .then(async (adddata) => {
                            if (adddata > 0) {
                                res.status(200).send(await middlewares.responseMiddleWares('module_added', true, undefined, 200));
                            } else {
                                res.status(400).send(await middlewares.responseMiddleWares('module_not_add', false, undefined, 400));
                            }
                        })
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.updatemodule = async (req, res) => {
    try {
        const module_id = req.body.module_id;

        if (module_id) {
            let data = {
                name: req.body.name,
                updated_by: req.user.id,
                updated_at: new Date(),
            }

            await knex('module_management')
                .where({ 'name': req.body.name })
                .whereNot({ 'id': module_id })
                .then(async (addata) => {
                    if (addata.length > 0) {
                        res.status(400).send(await middlewares.responseMiddleWares('module_already_available', false, undefined, 400))
                    } else {
                        await knex('module_management')
                            .update(data)
                            .where({ 'id': module_id })
                            .then(async (adddata) => {
                                if (adddata > 0) {
                                    res.status(200).send(await middlewares.responseMiddleWares('module_updated', true, undefined, 200));
                                } else {
                                    res.status(400).send(await middlewares.responseMiddleWares('module_not_update', false, undefined, 400));
                                }
                            })
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('module_id_not', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getmodule = async (req, res) => {
    try {
        const module_id = req.params.id;

        if (module_id) {
            await knex('module_management')
                .select('id', 'name')
                .where({ 'id': module_id })
                .then(async (moduledata) => {
                    if (moduledata.length > 0) {
                        moduledata = moduledata[0];
                        res.status(200).send(await middlewares.responseMiddleWares('get_module_data', true, moduledata, 200));
                    } else {
                        res.status(400).send(await middlewares.responseMiddleWares('module_not_found', false, undefined, 400));
                    }
                })
        } else {
            res.status(400).send(await middlewares.responseMiddleWares('module_id_not', false, undefined, 400));
        }
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getallmodule = async (req, res) => {
    try {
        await knex('module_management')
            .select('id', 'name')
            .then(async (moduledata) => {
                if (moduledata.length > 0) {
                    res.status(200).send(await middlewares.responseMiddleWares('get_module_data', true, moduledata, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('module_not_found', false, undefined, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}

module.exports.getmoduleaccess = async (req, res) => {
    try {
        let module = [];
        let data = {};
        await knex('module_access_management as access')
            .select('module.name')
            .leftJoin('module_management as module', 'access.module_id', 'module.id')
            .where({ 'user_id': req.user.id })
            .then(async (moduledata) => {
                if (moduledata.length > 0) {

                    for(let dat of moduledata) {
                        module.push(dat.name);
                    }

                    module.join(',');
                    data['module'] = module;

                    res.status(200).send(await middlewares.responseMiddleWares('get_module_data', true, data, 200));
                } else {
                    res.status(400).send(await middlewares.responseMiddleWares('module_not_found', false, undefined, 400));
                }
            })
    } catch (err) {
        console.log("err", err);
        res.status(400).send(await middlewares.responseMiddleWares('internal_error', false, undefined, 400));
    }
}