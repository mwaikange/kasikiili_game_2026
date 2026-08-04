const axios = require('axios');

let apiKey = process.env.SMSPORTAL_API_KEY;
let apiSecret = process.env.SMSPORTAL_API_SECRET;
let accountApiCredentials = apiKey + ':' + apiSecret;

let buff = new Buffer.from(accountApiCredentials);
let base64Credentials = buff.toString('base64');

let authHeader = 'Basic ' + base64Credentials;

let config = {
  headers: {
    'Authorization': authHeader,
  }
}

module.exports.sendsmsnotify = async (data) => {
  try {
    if (!apiKey || !apiSecret) {
      throw new Error('SMS Portal credentials are not configured');
    }

    await axios.get('https://rest.smsportal.com/authentication', config)
      .then(async response => {
        if (response.data) {
          console.log("res-sms", response.data);

          await Send(response.data.token, data.message, data.mobile_number);
        }
      })
      .catch(error => {
        if (error.response) {
          console.log(error.response.data);
        }
      });
  } catch (err) {
    console.log(err);
  }
}

function Send(token, message, destination) {
  let authHeader = 'Bearer ' + token;
  let config = {
    headers: {
      'Authorization': authHeader,
      'Content-Type': 'application/json'
    }
  }

  let data = JSON.stringify({
    messages: [{
      content: message,
      destination: destination
    }]
  })

  axios.post('https://rest.smsportal.com/bulkmessages', data, config)
    .then(response => {
      if (response.data) {
        console.log(response.data);
      }
    })
    .catch(error => {
      if (error.response) {
        console.log(error.response.data);
      }
    });
}
