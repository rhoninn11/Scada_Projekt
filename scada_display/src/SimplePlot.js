import React from 'react'
import { LineChart, Line, CartesianGrid, XAxis, YAxis } from 'recharts';
const data = [  { name: '1', uv: 400, pv: 2400, amt: 10 },
                { name: '2', uv: 300, pv: 2300, amt: 20 }, 
                { name: '3', uv: 300, pv: 1400, amt: 40 },
                { name: '4', uv: 200, pv: 2200, amt: 80 },
                { name: '5', uv: 280, pv: 4200, amt: 20 },
                { name: '6', uv: 200, pv: 2400, amt: 24 }];

const SimplePlot = (props) => {
    return (
        <LineChart width={600} height={300} data={data}>
            <Line type="monotone" dataKey={props.elo} stroke="#8884d8" />
            <CartesianGrid stroke="#ccc" />
            <XAxis dataKey="name" />
            <YAxis />
        </LineChart>
    )
}

export default SimplePlot