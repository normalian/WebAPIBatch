function atmarkitjob() {
    callwebsite("http://＜Webサイト名＞.azurewebsites.net/api/values/");
}

function callwebsite(url) {
    console.info("Executing job: call Yahoo API website");

    var req = require('request');
    req.post({
        headers: { 'Content-Type': 'application/json' },
        url: url,
        body: JSON.stringify({
            FromMailName: "＜fromユーザ名＞",
            FromMailAddress: "xxxxxxxx@hotmail.com",
            ToMailAddresses: [
                "yyyyyyyy@hotmail.com",
                "zzzzzzzz@hotmail.com"
            ]
        })
    },
    function (error, result, body) {
        if (body) {
            console.error("Job execute failed");
            console.error(body);
        } else {
            console.info("Job executed successfully");
        }
    });
}