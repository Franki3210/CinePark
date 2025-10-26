/**
 * =========================================================================
 * INICIALIZACIÓN PRINCIPAL DE LA APLICACIÓN
 * Este bloque se ejecuta una sola vez cuando el DOM está listo.
 * Orquesta todas las tareas de configuración inicial.
 * =========================================================================
 */
document.addEventListener('DOMContentLoaded', () => {

  // --- 1. CONFIGURACIÓN DE COMPONENTES GLOBALES ---
  
  // Inyecta el HTML del modal QR en el body una sola vez.
  // Esto asegura que siempre esté disponible para cualquier página que lo necesite.
  const modalQR_HTML = `
    <div id="modal-qr" class="modal-qr-overlay">
      <div class="modal-qr-content">
        <span id="modal-qr-close" class="modal-qr-close">&times;</span>
        <h3>¡Escanea para realizar tu pago!</h3>
        <img src="assets/img/QR.png" alt="Código QR de pago">
        <p id="modal-qr-timer" class="modal-qr-timer"></p>
      </div>
    </div>
  `;
  document.body.insertAdjacentHTML('beforeend', modalQR_HTML);
  
  // Configura el menú responsive (hamburguesa).
  const menuToggle = document.getElementById('menu-toggle');
  const nav = document.querySelector('.Navegacion');
  if (menuToggle && nav) {
    menuToggle.addEventListener('click', () => {
      menuToggle.classList.toggle('is-active');
      nav.classList.toggle('is-active');
    });
  }

  // --- 2. CONFIGURACIÓN DE MODALES (LOGIN Y REGISTRO) ---
  
  // Función reutilizable para manejar la lógica de cualquier modal.
  // Evita repetir el código para el modal de login y el de registro.
  const setupModal = (btnId, modalId, closeId) => {
    const btn = document.getElementById(btnId);
    const modal = document.getElementById(modalId);
    const closeBtn = document.getElementById(closeId);

    // Solo asigna listeners si los elementos existen (importante para cuando el usuario ya está logueado).
    if (btn && modal && closeBtn) {
      btn.addEventListener('click', () => {
        modal.style.display = 'flex';
      });
      closeBtn.addEventListener('click', () => {
        modal.style.display = 'none';
      });
      // Listener para cerrar el modal haciendo clic en el fondo.
      modal.addEventListener('click', (e) => {
        if (e.target === modal) {
          modal.style.display = 'none';
        }
      });
    }
  };

  // Usamos la función para configurar ambos modales.
  setupModal('btnRegistro', 'modalRegistro', 'cerrarModal');
  setupModal('btnLogin', 'modalLogin', 'cerrarLogin');

  // --- 3. CONFIGURACIÓN DE FORMULARIOS ---

  // Asignamos los listeners a los formularios de login y registro si existen.
  const formRegistro = document.getElementById("formRegistro");
  if (formRegistro) {
    formRegistro.addEventListener("submit", async (e) => {
      e.preventDefault();
      await registrarUsuario(e); // Llama a la función que ya tienes definida en otro lado.
    });
  }

  const formLogin = document.getElementById("formLogin");
  if (formLogin) {
    formLogin.addEventListener("submit", async (e) => {
      e.preventDefault();
      await loginUsuario(e); // Llama a la función que ya tienes definida en otro lado.
    });
  }
  
  // --- 4. EJECUCIÓN INICIAL DE LA APLICACIÓN ---
  
  // Verificamos si hay una sesión activa. Esta función decidirá si muestra
  // la interfaz de logueado o deja los botones de login/registro.
  verificarSesionAlCargar();
  
  // Cargamos el contenido inicial de la página principal (cartelera).
  // No es necesario llamar a cargarPeliculas() por separado, ya que
  // inicializarPaginaPrincipal() ya lo hace.
  inicializarPaginaPrincipal();
});