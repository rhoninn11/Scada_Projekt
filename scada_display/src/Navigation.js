import React from 'react'
import { Navbar, Badge } from 'react-bootstrap';


const Navigation = () => {
    return (
        <Navbar bg="light" expand="lg">
            <Navbar.Brand href="#home"><h1>Scada project</h1></Navbar.Brand>
            <Badge variant="secondary">Created by Leszek & Patryk</Badge>
        </Navbar>
    )
}

export default Navigation;