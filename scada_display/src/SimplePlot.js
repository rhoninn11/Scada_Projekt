import React from 'react'
import { LineChart, Line, CartesianGrid, XAxis, YAxis } from 'recharts';
import SensorDataContainer from './SensorDataContainer'
import { Subscribe } from 'unstated';

const SimplePlot = (props) => {
    return (
        <Subscribe to={[SensorDataContainer]}>
            {context => (
                <LineChart width={600} height={300} data={context.state.data}>
                    <Line type="monotone" dataKey={props.elo} stroke="#8884d8" />
                    <CartesianGrid stroke="#ccc" />
                    <XAxis dataKey="name" />
                    <YAxis />
                </LineChart>)}
        </Subscribe>)
}

export default SimplePlot