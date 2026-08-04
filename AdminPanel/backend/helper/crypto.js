const { Buffer } = require('buffer');

const encrypt = (value) => {
    if (value) {
        return Buffer.from(JSON.stringify(value)).toString('base64');
    }
}

const decrypt = (value) => {
    if (value) {
        return JSON.parse(Buffer.from(value, 'base64').toString('ascii'));
    }
}

module.exports = { encrypt, decrypt };