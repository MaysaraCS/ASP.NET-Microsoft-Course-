// ============================================================
// CONSTANTS
// ============================================================

// STEP 1: Define a single key for localStorage — keeps naming consistent everywhere
const STORAGE_KEY = "taskApp_tasks";


// ============================================================
// CORE STORAGE FUNCTIONS
// ============================================================

// STEP 2: Save the entire tasks array to localStorage
// JSON.stringify converts the array/object into a plain string (localStorage only holds strings)
function saveTasksToLocalStorage(tasks) {
    try {
        const json = JSON.stringify(tasks);          // e.g. '[{"name":"Buy milk","priority":"High","completed":false}]'
        localStorage.setItem(STORAGE_KEY, json);     // store under our key
    } catch (error) {
        // STEP 3: Catch QuotaExceededError or any serialization problem
        console.error("Failed to save tasks to localStorage:", error);
    }
}

// STEP 4: Load tasks from localStorage and return them as a real JS array
// Returns [] if nothing is stored yet (new user) or if the data is corrupted
function loadTasksFromLocalStorage() {
    try {
        const json = localStorage.getItem(STORAGE_KEY); // returns null if key doesn't exist

        // STEP 5: null means the user has never saved tasks — return a clean empty array
        if (json === null) return [];

        // STEP 6: JSON.parse turns the stored string back into a real JS array/object
        return JSON.parse(json);
    } catch (error) {
        // STEP 7: JSON.parse throws SyntaxError if the stored data is corrupted
        // Wipe the bad data and start fresh rather than crashing the app
        console.error("Corrupted localStorage data — resetting:", error);
        localStorage.removeItem(STORAGE_KEY);
        return [];
    }
}

// STEP 8: Add a single new task without overwriting the others
// completed defaults to false — a brand-new task is never done yet
function addTaskToLocalStorage(taskName, priority, completed = false) {
    const tasks = loadTasksFromLocalStorage();   // load what's already there

    // STEP 9: Build a task object with a timestamp so we can sort by date later
    const newTask = {
        id:        Date.now(),          // unique numeric ID (milliseconds since epoch)
        name:      taskName,
        priority:  priority,
        completed: completed,
        createdAt: new Date().toISOString()  // e.g. "2024-03-15T10:30:00.000Z"
    };

    tasks.push(newTask);                // add to the array
    saveTasksToLocalStorage(tasks);     // persist the updated array
    return newTask;                     // return it so the UI can use it immediately
}

// STEP 10: Flip one task's completed flag using its array index
function markTaskCompleteInLocalStorage(taskIndex) {
    const tasks = loadTasksFromLocalStorage();

    // STEP 11: Guard against an out-of-range index (e.g. stale UI)
    if (taskIndex < 0 || taskIndex >= tasks.length) {
        console.warn("markTaskCompleteInLocalStorage: index out of range", taskIndex);
        return;
    }

    tasks[taskIndex].completed = !tasks[taskIndex].completed;  // toggle true ↔ false
    saveTasksToLocalStorage(tasks);     // save the change back
}

// STEP 12: Delete a task by its index and re-save
function deleteTaskFromLocalStorage(taskIndex) {
    const tasks = loadTasksFromLocalStorage();
    tasks.splice(taskIndex, 1);         // remove exactly 1 element at taskIndex
    saveTasksToLocalStorage(tasks);
}

// STEP 13: Wipe all stored tasks — useful for a "Clear All" button
function clearAllTasksFromLocalStorage() {
    localStorage.removeItem(STORAGE_KEY);
}


// ============================================================
// UI FUNCTIONS
// ============================================================

// STEP 14: Build the task list in the DOM from whatever is in localStorage
function renderTasks() {
    const tasks    = loadTasksFromLocalStorage();
    const list     = document.getElementById("taskList");
    const empty    = document.getElementById("emptyMessage");

    list.innerHTML = "";   // clear old items before re-rendering

    // STEP 15: Show a friendly message when there are no tasks at all
    if (tasks.length === 0) {
        empty.style.display = "block";
        return;
    }
    empty.style.display = "none";

    // STEP 16: Loop through every task and create a list item for it
    tasks.forEach((task, index) => {

        const li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between align-items-center";

        // STEP 17: Strike through completed tasks so status is immediately obvious
        const nameStyle = task.completed ? "text-decoration: line-through; color: #888;" : "";

        // STEP 18: Priority badge colour — High=red, Medium=yellow, Low=green
        const badgeColor = task.priority === "High"   ? "danger"
                         : task.priority === "Medium" ? "warning"
                         :                              "success";

        li.innerHTML = `
            <div>
                <span style="${nameStyle}">${task.name}</span>
                <span class="badge bg-${badgeColor} ms-2">${task.priority}</span>
                ${task.completed ? '<span class="badge bg-secondary ms-1">Done</span>' : ""}
            </div>
            <div>
                <!-- STEP 19: Each button passes the array index to the right handler -->
                <button class="btn btn-sm btn-outline-success me-1"
                        onclick="handleToggleComplete(${index})">
                    ${task.completed ? "Undo" : "Complete"}
                </button>
                <button class="btn btn-sm btn-outline-danger"
                        onclick="handleDelete(${index})">
                    Delete
                </button>
            </div>
        `;
        list.appendChild(li);
    });

    // STEP 20: Keep the counter badge up to date
    document.getElementById("taskCount").textContent = tasks.length;
}

// STEP 21: Handle the Add Task form submission
function handleAddTask() {
    const nameInput     = document.getElementById("taskName");
    const priorityInput = document.getElementById("priority");
    const feedback      = document.getElementById("formFeedback");

    const name     = nameInput.value.trim();
    const priority = priorityInput.value;

    // STEP 22: Client-side validation — same idea as the TempData lab, but in JS
    if (!name) {
        feedback.className  = "alert alert-danger mt-2";
        feedback.textContent = "⚠️ Task name cannot be empty.";
        feedback.style.display = "block";
        return;
    }

    addTaskToLocalStorage(name, priority);   // STEP 23: persist the new task

    // STEP 24: Show success feedback and clear the inputs
    feedback.className   = "alert alert-success mt-2";
    feedback.textContent = `✅ "${name}" (${priority}) added and saved!`;
    feedback.style.display = "block";

    nameInput.value = "";
    setTimeout(() => { feedback.style.display = "none"; }, 3000); // hide after 3 s

    renderTasks();   // STEP 25: re-render so the new task appears immediately
}

// STEP 26: Toggle complete and re-render
function handleToggleComplete(index) {
    markTaskCompleteInLocalStorage(index);
    renderTasks();
}

// STEP 27: Delete and re-render
function handleDelete(index) {
    deleteTaskFromLocalStorage(index);
    renderTasks();
}

// STEP 28: Clear everything and re-render
function handleClearAll() {
    if (confirm("Delete ALL tasks? This cannot be undone.")) {
        clearAllTasksFromLocalStorage();
        renderTasks();
    }
}

// STEP 29: When the page first loads, render whatever is already in localStorage
// This is what makes data survive a browser close/reopen
document.addEventListener("DOMContentLoaded", renderTasks);