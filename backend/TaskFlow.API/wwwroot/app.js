const $ = id => document.getElementById(id);
let taskToDelete = null;

function authenticated() { return sessionStorage.getItem('taskflow-token'); }
function show(view) { $('login-view').classList.toggle('hidden', view !== 'login'); $('app-view').classList.toggle('hidden', view !== 'app'); }
function message(id, text, error = false) { const el = $(id); el.textContent = text; el.classList.toggle('error', error); }

$('login-form').addEventListener('submit', async event => {
  event.preventDefault();
  message('login-error', '');
  const email = $('email').value.trim();
  const password = $('password').value;
  if (!email || !password) return message('login-error', 'Complete el correo y la contrasena.', true);
  try {
    const response = await fetch('/api/auth/login', { method:'POST', headers:{'Content-Type':'application/json'}, body:JSON.stringify({email,password}) });
    const data = await response.json();
    if (!response.ok) return message('login-error', data.message || 'No fue posible iniciar sesion.', true);
    sessionStorage.setItem('taskflow-token', data.token);
    show('app');
    await loadTasks();
  } catch { message('login-error', 'No hay conexion con el servidor.', true); }
});

$('logout-button').addEventListener('click', () => { sessionStorage.clear(); $('login-form').reset(); show('login'); });
$('refresh-button').addEventListener('click', loadTasks);
$('cancel-edit').addEventListener('click', resetForm);

$('task-form').addEventListener('submit', async event => {
  event.preventDefault();
  const id = $('task-id').value;
  const title = $('task-title').value.trim();
  const description = $('task-description').value.trim();
  if (title.length < 3 || title.length > 100) return message('form-message', 'El titulo debe tener entre 3 y 100 caracteres.', true);
  const payload = { title, description, completed:$('task-completed').checked };
  const response = await fetch(id ? `/api/tasks/${id}` : '/api/tasks', {method:id?'PUT':'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
  if (!response.ok) return message('form-message', 'No fue posible guardar la tarea.', true);
  resetForm();
  message('form-message', id ? 'Tarea actualizada correctamente.' : 'Tarea creada correctamente.');
  await loadTasks();
});

async function loadTasks() {
  const response = await fetch('/api/tasks');
  const tasks = await response.json();
  $('task-count').textContent = `${tasks.length} ${tasks.length === 1 ? 'tarea' : 'tareas'}`;
  $('empty-state').classList.toggle('hidden', tasks.length > 0);
  const list = $('task-list'); list.innerHTML = '';
  tasks.forEach(task => {
    const item = document.createElement('article'); item.className = `task${task.completed ? ' done' : ''}`; item.dataset.taskId = task.id;
    const copy = document.createElement('div');
    const title = document.createElement('h3'); title.textContent = task.title;
    const description = document.createElement('p'); description.textContent = task.description || 'Sin descripcion';
    const status = document.createElement('small'); status.textContent = task.completed ? 'Completada' : 'Pendiente';
    copy.append(title, description, status);
    const buttons = document.createElement('div'); buttons.className = 'task-buttons';
    const edit = document.createElement('button'); edit.textContent = 'Editar'; edit.className = 'secondary edit-task'; edit.onclick = () => editTask(task);
    const remove = document.createElement('button'); remove.textContent = 'Eliminar'; remove.className = 'danger delete-task'; remove.onclick = () => openDelete(task.id);
    buttons.append(edit, remove); item.append(copy, buttons); list.append(item);
  });
}

function editTask(task) {
  $('task-id').value=task.id; $('task-title').value=task.title; $('task-description').value=task.description; $('task-completed').checked=task.completed;
  $('form-title').textContent='Editar tarea'; $('completed-label').classList.remove('hidden'); $('cancel-edit').classList.remove('hidden'); message('form-message',''); $('task-title').focus();
}
function resetForm(){ $('task-form').reset(); $('task-id').value=''; $('form-title').textContent='Nueva tarea'; $('completed-label').classList.add('hidden'); $('cancel-edit').classList.add('hidden'); }
function openDelete(id){ taskToDelete=id; $('confirm-modal').classList.remove('hidden'); }
$('cancel-delete').onclick=()=>{ taskToDelete=null; $('confirm-modal').classList.add('hidden'); };
$('confirm-delete').onclick=async()=>{ await fetch(`/api/tasks/${taskToDelete}`,{method:'DELETE'}); $('confirm-modal').classList.add('hidden'); taskToDelete=null; message('form-message','Tarea eliminada correctamente.'); await loadTasks(); };

if (authenticated()) { show('app'); loadTasks(); } else show('login');
