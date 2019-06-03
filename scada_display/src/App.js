import React from 'react';
import logo from './logo.svg';
import './App.css';
import Navigation from './Navigation'
import SimplePlot from './SimplePlot'
import SensorData from './SensorData'

function App() {
  return (
    <div className="App">
      <Navigation></Navigation>
      <header className="App-header">
        <SensorData>
          <SimplePlot elo='uv'></SimplePlot>
          <SimplePlot elo='pv'></SimplePlot>
          <SimplePlot elo='amt'></SimplePlot>
        </SensorData>


      </header>
    </div>
  );
}

export default App;
