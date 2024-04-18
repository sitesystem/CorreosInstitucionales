var express = require('express');

var app = express.createServer();

app.get('/api/SendWhatsApp', function (req, res) {
    res.send('Hola WhatsApp');
});

app.listen(process.env.PORT);
