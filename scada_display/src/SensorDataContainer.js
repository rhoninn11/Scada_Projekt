import { Container, } from 'unstated';
import c3 from 'c3';
import axios from 'axios';


class SensorDataContainer extends Container {

    constructor() {
        super();
        setInterval(this.generateData, 50)
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
    mainSensor = []
    sensor1 = []
    sensor2 = []
    sensor3 = []
    ch1 = null
    ch2 = null
    ch3 = null
    chmain = null


    generateData = () => {


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
            this.chmain = c3.generate({
                bindto: '#main',
                data: { columns: [['Proccesed', ...this.mainSensor]] }
            });
            console.log('elo');

        }

        this.statusUpdated = false;
        axios.get('http://localhost:5000/data').then(result => {

            let data = JSON.parse(result.data)

            let newMain = data['main']
            let new1 = data['sensor1']
            let new2 = data['sensor2']
            let new3 = data['sensor3']
            console.log(new1);
            console.log(new2);
            console.log(new3);
            console.log(newMain);
            

            this.sensor1.push(...new1);
            this.sensor2.push(...new2);
            this.sensor3.push(...new3);
            this.mainSensor.push(...newMain);

            while (this.sensor1.length > 100) {
                this.sensor1.shift();
            }
    
            while (this.sensor2.length > 100) {
                this.sensor2.shift();
            }
    
            while (this.sensor3.length > 100) {
                this.sensor3.shift();
            }

            while (this.mainSensor.length > 100) {
                this.mainSensor.shift();
            }
    
            this.chmain.load({
                columns: [['Proccesed', ...this.mainSensor]]
            });
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

        }).catch(error => console.log(error))
    }
}

export default SensorDataContainer