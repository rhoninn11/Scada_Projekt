const express = require('express')
const app = express()
const net = require('net')
const port = 5000

let sensor1Data = []
let sensor2Data = []
let sensor3Data = []
let sensorProcessedData = []

const dataServer = net.createServer(socket => {
    console.log('witam');

    socket.on('data', function (chunk) {
        let data = JSON.parse(chunk.toString('utf8'))

        sensorProcessedData.push(data['ProcessedData'])
        if(sensorProcessedData.length > 500){
            sensorProcessedData.shift();
        }

        let sarr = data['SensorData'];
        console.log(sarr.length);
        if(sarr.length == 1){
            console.log('elo',sarr[0]);
            sensor1Data.push(sarr[0]);
            if(sensor1Data > 500){
                sensor1Data.shift();
            }
        }
        else if(sarr.lengt == 2){
            console.log('elo',sarr[0]);
            console.log('elo2',sarr[1]);
        }
        else if(sarr.lengt == 3){
            console.log('elo',sarr[0]);
            console.log('elo2',sarr[1]);
            console.log('elo3',sarr[2]);
        }

        // if(sarr.lengt == 2){
        //     console.log('elo2',sarr[1]);
        //     sensor2Data.push(sarr[1]);
        //     if(sensor2Data > 500){
        //         sensor2Data.shift();
        //     }
        // };

        // if(sarr.lengt >= 3){
        //     console.log('elo3',sarr[2]);
        //     sensor3Data.push(sarr[2]);
        //     if(sensor3Data > 500){
        //         sensor3Data.shift();
        //     }
        // };
    });
})
app.use(express.static('../scada_display/build'))
app.get('/', (req, res) => res.send('Hello World!'))

app.get('/data', (req, res) => {

    console.log("data requested");
    let jsonData = {
        main: [...sensorProcessedData],
        sensor1: [...sensor1Data],
        sensor2: [...sensor2Data],
        sensor3: [...sensor3Data],
    };
    
    sensor1Data = [];
    sensor2Data = [];
    sensor3Data = [];
    sensorProcessedData = [];

    res.json(JSON.stringify(jsonData));
})
dataServer.listen(1337, '127.0.0.1')
app.listen(port, () => console.log(`Example app listening on port ${port}!`))