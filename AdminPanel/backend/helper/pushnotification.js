const config = require('./config');
const knex = require('knex')(require('../helper/db'));
const middlewares = require('./middlewares');

var FCM = require('fcm-node');
var fcm = new FCM('AAAAaYfLEoc:APA91bFxQNDrGg6yG_siz56pon9zTHVWWUAdJ6eFn4zW0o8gl0heibP2y6lHy2E1VO9ew4pEQKdqG6fZGDOVj8uToLqzhgDTI09d1NBMicuUPJf3X14SNXTh-D9DuXe2gqJpZvBFok8a');

const notification = require('../controller/notifications');


module.exports.sendNotification = async (type, id, notiData) => {
    try {
        if (type == '1') {
            const notify_data = {
                user_type: type,
                title: notiData.title,
                message: notiData.body,
                user_id: id,
                created_date: notiData.created_date,
                created_by: notiData.created_by
            }
            await notification.add(notify_data);
            await knex('User device')
                .distinct()
                .select('fcm_token')
                .where({ user_id: id })
                .then(async (fetchData) => {
                    let message;
                    for (const token of fetchData) {
                        message = {
                            to: token.fcm_token,
                            notification: {
                                icon: `${config.image_url}/site-logo.jpg`,
                                title: notiData.title,
                                body: notiData.body
                            }
                        };
                        await fcm.send(message, function (err, response) {
                            if (err) console.log("Something has gone wrong!");
                            else console.log("Successfully sent with response: ");
                        });
                    }
                })
        } else if (type == '2') {
            const notify_data = {
                user_type: type,
                title: notiData.title,
                message: notiData.body,
                user_id: id,
                created_date: notiData.created_date,
                created_by: notiData.created_by
            }
            await notification.add(notify_data);

            await knex('User device')
                .distinct()
                .select('fcm_token')
                .where({ user_id: id })
                .then(async (fetchData) => {

                    let message;
                    for (const token of fetchData) {
                        message = {
                            to: token.fcm_token,
                            notification: {
                                icon: `${config.image_url}/site-logo.jpg`,
                                title: notiData.title,
                                body: notiData.body
                            }
                        };
                        await fcm.send(message, function (err, response) {
                            if (err) console.log("Something has gone wrong!");
                            else console.log("Successfully sent with response: ");
                        });
                    }
                })
        } else {
            const notify_data = {
                user_type: type,
                title: notiData.title,
                message: notiData.body,
                user_id: id,
                created_date: notiData.created_date,
                created_by: notiData.created_by
            }
            await notification.add(notify_data);
            await knex('User device')
                .distinct()
                .select('fcm_token')
                .where({ user_id: id })
                .then(async (fetchData) => {
                    let message;
                    for (const token of fetchData) {
                        message = {
                            to: token.fcm_token,
                            notification: {
                                icon: `${config.image_url}/site-logo.jpg`,
                                title: notiData.title,
                                body: notiData.body
                            }
                        };
                        await fcm.send(message, function (err, response) {
                            if (err) console.log("Something has gone wrong!");
                            else console.log("Successfully sent with response: ");
                        });
                    }
                })
        }
    } catch (error) {
        console.log(error);
    }
}