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
          <SensorData>
            <SimplePlot elo='uv'></SimplePlot>
            <SimplePlot elo='pv'></SimplePlot>
            <SimplePlot elo='amt'></SimplePlot>
          </SensorData>


        </header>
      </div>
    </Provider>
  );
}

export default App;
