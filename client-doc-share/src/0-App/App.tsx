import React, { useState, useEffect } from "react";
import { Outlet } from "react-router-dom";
import Header from "../4-Widgets/Header/Header";
import SidePanel from "../4-Widgets/SidePanel/SidePanel";
import "./App.css";

const App: React.FC = () => {
  return (
    <div className="app">
      <Header />

      <div className="main-container">
        <div className="outlet">
          <Outlet />
        </div>
        <div className="side-panel">
          <SidePanel />
        </div>
      </div>
    </div>
  );
};

export default App;
