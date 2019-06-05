import React from 'react';
import logo from './logo.svg';
import './App.css';
import Navigation from './Navigation'
import SimplePlot from './SimplePlot'
import SensorData from './SensorData'
import ButtonBar from './ButtonBar'
import { Provider, Subscribe } from 'unstated';


function App() {
  return (
    <Provider>
     

      <div className="App">
        <Navigation></Navigation>
        <ButtonBar></ButtonBar>
        <header className="App-header">
          <SensorData title="Sensor Data">
            <SimplePlot id={'sensor1'}></SimplePlot>
            <SimplePlot id={'sensor2'}></SimplePlot>
            <SimplePlot id={'sensor3'}></SimplePlot>
          </SensorData>
          <SensorData title="ProccessedSignal">
            <SimplePlot id={'main'}></SimplePlot>
            <div id="elo1"></div>
          </SensorData>


        </header>
      </div>
    </Provider>
  );
}

export default App;
