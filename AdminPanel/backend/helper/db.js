const config = require('./config');

module.exports = {
    client: 'mysql',
    connection: config.connection,
    debug: false,
    pool: {
        min: 1,
        max: 10,
        afterCreate: function (conn, cb) {
            conn.query('SET sql_mode="NO_ENGINE_SUBSTITUTION";', function (err) {
                cb(err, conn);
            });
        }
    },
    acquireConnectionTimeout: 10000
}