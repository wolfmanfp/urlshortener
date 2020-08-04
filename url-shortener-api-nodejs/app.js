var express = require('express');
var apiRouter = require('./routes/api');
var app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: false }));

app.use('/api', apiRouter);

module.exports = app;
