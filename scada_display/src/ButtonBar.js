import React from 'react'
import { Button, ButtonToolbar } from 'react-bootstrap';
import SensorDataContainer from './SensorDataContainer'
import { Subscribe } from 'unstated';


const ButtonBar = () => {
    return (
        <Subscribe to={[SensorDataContainer]}>
            {context => (
                <ButtonToolbar>
                    <Button variant="primary" size="lg" active onClick={context.generateData}>
                        Primary button
                    </Button>
                    <Button variant="primary" size="lg" active>
                        Button
                    </Button>
                </ButtonToolbar>)}
        </Subscribe>)

}


export default ButtonBar