import { Container, } from 'unstated';
import c3 from 'c3';
import axios from 'axios';


class SensorDataContainer extends Container {

    constructor() {
        super();
    }
    state = {
        generationStatus: false,
        sensor1: [1, 1, 1],
        sensor2: [0, 0, 0],
        sensor3: [-1, -1, -1],
        elo: 1

    }
    sio = null;
    interval = undefined;
    lastTimestamp = null;
    statusUpdated = true;
    sensor1 = []
    sensor2 = []
    sensor3 = []
    ch1 = null
    ch2 = null
    ch3 = null


    generateData = () => {
        
        axios.get('http://localhost:5000/data').then(result => {
            console.log(result);
        })

        if (!this.statusUpdated) {
            return;
        }

        if (this.ch1 == null || this.ch2 == null || this.ch3 == null) {
            this.ch1 = c3.generate({
                bindto: '#sensor1',
                data: { columns: [['sensor1', ...this.sensor1]] }
            });
            this.ch2 = c3.generate({
                bindto: '#sensor2',
                data: { columns: [['sensor2', ...this.sensor2]] }
            });
            this.ch3 = c3.generate({
                bindto: '#sensor3',
                data: { columns: [['sensor3', ...this.sensor3]] }
            });
            console.log('elo');

        }

        let time_points = 0;

        if (this.lastTimestamp == null) {
            this.lastTimestamp = Date.now();
            time_points = 1;
        }

        let timeStump = Date.now()
        let timeDelta = timeStump - this.lastTimestamp;
        this.lastTimestamp = timeStump;

        console.log(timeDelta);


        let sin1Value = Math.sin(2 * Math.PI * 0.03 * timeStump / 1000)
        let sin2Value = Math.sin(2 * Math.PI * 0.2 * timeStump / 1000)
        let sin3Value = Math.sin(2 * Math.PI * 0.13 * timeStump / 1000)

        this.statusUpdated = false;
        this.sensor1.push(sin1Value);
        this.sensor2.push(sin2Value);
        this.sensor3.push(sin3Value);

        while (this.sensor1.length > 100) {
            this.sensor1.shift();
        }

        while (this.sensor2.length > 100) {
            this.sensor2.shift();
        }

        while (this.sensor3.length > 100) {
            this.sensor3.shift();
        }

        this.ch1.load({
            columns: [['sensor1', ...this.sensor1]]
        });
        this.ch2.load({
            columns: [['sensor2', ...this.sensor3]]
        });
        this.ch3.load({
            columns: [['sensor3', ...this.sensor3]]
        });
        this.statusUpdated = true;
    }
}

export default SensorDataContainer