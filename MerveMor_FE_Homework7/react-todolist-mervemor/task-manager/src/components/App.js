import React from 'react';
import TaskList from './TaskList';
import TaskForm from './TaskForm';
import Header from './Header';
import TaskListContextProvider from '../context/TaskListContext';
import '../App.css';
import Kodluyoruz from "../images/kodluyoruz.png";

const App = () => {
    return (
    <TaskListContextProvider>
    <div className = "container"> 
        <div className = "app-wrapper">
        <img src={Kodluyoruz} className="img-fluid" alt="kodluyoruz"></img>
            <Header/>
            <div className = "main">
            <TaskForm/>
            <TaskList />
            </div>
            
        </div>
    </div>
    </TaskListContextProvider>
    );
};

export default App;
