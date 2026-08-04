const cryptojs = require('../helper/crypto');

module.exports.encrypted = async(req, res)=>{
    res.send({info:cryptojs.encrypt(req.body)})
}

module.exports.decrypted = async(req, res)=>{
    res.send(cryptojs.decrypt(req.body.info))
}