const form = document.querySelector('#add-task');
const input = document.querySelector('#todo');
const taskList = document.querySelector('#task-list');
let taskID = 0;

taskList.addEventListener('click', function(e) {
    if (e.target.tagName === 'BUTTON') {
        e.target.parentElement.remove();
        let inputTask = document.getElementById('todo');
        localStorage.setItem('email', inputTask.value);
    } else if (e.target.tagName === 'LI') {
        e.target.classList.toggle('task-complete');
    }
});

form.addEventListener('submit', function(e) {
    e.preventDefault();
    const newTask = document.createElement('li');
    const removeBtn = document.createElement('button');
    let savedInput = input.value;
    removeBtn.innerText = 'Remove Task';
    newTask.innerText = input.value;
    newTask.appendChild(removeBtn);
    taskList.appendChild(newTask);
    localStorage.setItem('Task'+taskID, input.value);
    taskID++;
    input.value = '';
});

function showSavedToDos() {
    const keys = Object.keys(localStorage);
    let i = keys.length;

    while (i--) {
        const newTask = document.createElement('li');
        const removeBtn = document.createElement('button');
        removeBtn.innerText = 'Remove Task';
        newTask.innerText = localStorage.getItem(keys[i]);
        newTask.appendChild(removeBtn);
        taskList.appendChild(newTask);
    }
}
showSavedToDos();