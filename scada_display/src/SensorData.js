import React from 'react'
import { Card, ListGroup } from 'react-bootstrap';

const SensorData = (props) => {
    console.log(props.children);

    return (
        <Card style={{width: '20em'}}>
            <Card.Header>{props.title}</Card.Header>
            <ListGroup variant="flush">
                {props.children.map((child) => <ListGroup.Item key={child.props.id}>{child}</ListGroup.Item>)}
            </ListGroup>
        </Card>
    )
}

export default SensorData;