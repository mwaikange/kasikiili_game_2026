require('dotenv').config()

module.exports = {
    port: process.env.PORT || 3008,
    connection: {
        port: process.env.DBCONNECTION_PORT,
        host: process.env.DBCONNECTION_HOST,
        user: process.env.DBCONNECTION_USER,
        password: process.env.DBCONNECTION_PASSWORD,
        database: process.env.DBCONNECTION_DB,
        connectTimeout: 3600
    },
    secret_key: process.env.JWT_SECRET,
};
