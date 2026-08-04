const config = require('./helper/config');
const http = require('http');
const https = require('https');
const express = require('express')
const bodyParser = require('body-parser')
const route = require('./router/app');
const cors = require('cors');

const app = express();
const server = http.createServer(app);

app.use(bodyParser.json({ limit: '100mb' }));
app.use(bodyParser.urlencoded({ limit: '100mb', extended: true, parameterLimit: 1000000 }));

app.use((req, res, next) => {
    res.header("Access-Control-Allow-Origin", "*");
    res.header(
        "Access-Control-Allow-Headers",
        "Origin, X-Requested-With, Content-Type, Accept, Authorization"
    );
    if (req.method === 'OPTIONS') {
        res.header('Access-Control-Allow-Methods', 'PUT, POST, PATCH, DELETE, GET');
        return res.status(200).json({});
    }
    next();
});

app.use(express.static(__dirname + '/imagefolder'));
app.use(cors());

app.get('/', (req, res) => {
    res.send("Welcome to the KASLKILI");
})

app.use('/kaslkili', route);

server.listen(config.port, (err) => {
    if (err) throw (err);
    console.log('Server Up And Working', config.port);
});