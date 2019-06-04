import { Container } from 'unstated';

class SensorDataContainer extends Container {
    state = {
        generationStatus: false,
        data: [{ name: '1', uv: 400, pv: 2400, amt: 10 },
        { name: '2', uv: 300, pv: 2300, amt: 20 },
        { name: '3', uv: 300, pv: 1400, amt: 40 },
        { name: '4', uv: 200, pv: 2200, amt: 80 },
        { name: '5', uv: 280, pv: 4200, amt: 20 },
        { name: '6', uv: 200, pv: 2400, amt: 24 }]
    }

    generateData = () => {
        let generationStatus = this.state.generationStatus
        console.log(generationStatus);
        this.setState({ data: [...this.state.data, { name: '3', uv: 300, pv: 1400, amt: 40 }] })

    }
}

export default SensorDataContainer