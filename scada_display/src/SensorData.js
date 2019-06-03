import React from 'react'
import { Card, ListGroup } from 'react-bootstrap';

const SensorData = (props) => {
    console.log(props.children);

    return (
        <Card>
            <Card.Header>Sensors Data</Card.Header>
            <ListGroup variant="flush">
                {props.children.map((child) => <ListGroup.Item>{child}</ListGroup.Item>)}
            </ListGroup>
        </Card>
    )
}

export default SensorData;