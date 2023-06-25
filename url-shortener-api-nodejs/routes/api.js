const express = require('express');
const crypto = require('crypto');
const redis = require('redis');
const base62 = require('base-62.js');

const router = express.Router();
const client = redis.createClient({
    host: '127.0.0.1',
    port: '6379',
    password: 'redis',
    db: 0
});

client.on('error', err => console.error(err));
client.connect();

router.get('/url/:hash', async function (req, res) {
    let url = await client.get(req.params.hash);
    res.status(200).json({url: url}).end();
});

router.post('/url', async function (req, res) {
    let url = req.body.url;
    let hash = createHash(url);
    await client.set(hash, url);

    res.status(201).json({hash: hash}).end();
});

function createHash(url) {
    return base62.encodeHex(createMd5Hash(url)).substring(0, 6);
}

function createMd5Hash(url) {
    return crypto.createHash('md5').update(url).digest("hex");
}

module.exports = router;