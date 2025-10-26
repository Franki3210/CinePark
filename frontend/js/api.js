// ✅ Registrar usuario (Sin cambios)
async function registrarUsuario(e) {
  // ... tu código de registro va aquí ...
  const nombre = document.getElementById("nombre").value.trim();
  const apellidoPaterno = document
    .getElementById("apellidoPaterno")
    .value.trim();
  const apellidoMaterno = document
    .getElementById("apellidoMaterno")
    .value.trim();
  const numeroDocumento = document
    .getElementById("numeroDocumento")
    .value.trim();
  const telefono = document.getElementById("telefono").value.trim();
  const fechaNacimiento = document.getElementById("fechaNacimiento").value;
  const genero = document.getElementById("genero").value;
  const email = document.getElementById("email").value.trim();
  const contrasena = document.getElementById("contrasena").value;
  const confirmarContrasena = document.getElementById(
    "confirmarContrasena"
  ).value;

  if (contrasena !== confirmarContrasena) {
    mostrarVentana("❌ Las contraseñas no coinciden.", "negro");
    return;
  }

  const url = new URL("http://localhost:5235/api/Personas/registrar");
  const params = new URLSearchParams({
    nombre,
    apellidoPaterno,
    apellidoMaterno,
    numeroDocumento,
    telefono,
    fechaNacimiento,
    genero,
    email,
    contrasena,
  });
  url.search = params.toString();

  try {
    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
    });

    const result = await response.json();

    if (response.ok) {
      mostrarVentana("Registro exitoso", "negro");
      modalRegistro.style.display = "none";
      e.target.reset();
    } else {
      mostrarVentana("Error: " + (result.error || "Error desconocido"), "rojo");
    }
  } catch (error) {
    mostrarVentana("Error de red: " + error.message, "rojo");
  }
}
function mostrarInterfazDeUsuarioLogueado(usuario) {
  // 1. Obtenemos los elementos del DOM
  const nombre_user = document.querySelector(".header-actions");
  const nav = document.querySelector(".Navegacion");

  // 2. Eliminamos los botones
  document.getElementById("btnLogin")?.remove();
  document.getElementById("btnRegistro")?.remove();
  document.getElementById("modalLogin")?.remove();

  // 3. Mostramos el nombre del usuario y un botón de "Cerrar Sesión"
  // (He añadido una clase para controlar mejor el estilo del h2 desde el CSS si quieres)
  const nombreUsuario = usuario.email.split("@")[0];
  nombre_user.innerHTML = `
    <h2 class="nombre-usuario">${nombreUsuario} 👤</h2>
    <button id="btnLogout" class="btn-login" style="background-color: #D90429;">Cerrar Sesión</button>
  `;

  // 4. Creamos la barra de navegación según los roles
  const { roles } = usuario;
  let navContent = "";
  if (roles.cliente) {
    navContent += `
      <a href="#cartelera" class="nav-link">Cartelera</a>
      <a href="pages/purchase_history.html" class="nav-link">Historial de compra</a>
      <a href="pages/function.html" class="nav-link">Funciones</a>
      `;
  }
  if (roles.administrativo) {
    navContent += `
      <a href="pages/movie.html" class="nav-link">Películas</a>
    `;
  }
  nav.innerHTML = navContent;

  // ====================== LA CORRECCIÓN CLAVE ESTÁ AQUÍ ======================
  // Quitamos cualquier estilo en línea para que el CSS externo tome el control.
  nav.removeAttribute('style');
  // ========================================================================

  // 5. Activamos la navegación SPA para los nuevos enlaces
  document.querySelectorAll(".nav-link").forEach((link) => {
    link.addEventListener("click", (evento) => {
      evento.preventDefault();

      // --- MEJORA DE UX: Cerrar menú móvil al hacer clic ---
      document.querySelector('.Navegacion').classList.remove('is-active');
      document.querySelector('.menu-toggle').classList.remove('is-active');
      // --------------------------------------------------

      const ruta = link.getAttribute("href");
      if (ruta === "#cartelera") {
        mainContent.innerHTML = mainContentOriginalHTML;
        inicializarPaginaPrincipal();
      } else {
        cargarContenidoExterno(ruta);
      }
    });
  });

  // 6. Añadimos la funcionalidad al botón de Cerrar Sesión
  document.getElementById("btnLogout").addEventListener("click", () => {
    localStorage.removeItem("usuario"); // Limpiamos el localStorage
    window.location.reload(); // Recargamos la página para resetear el estado
  });
}

// --- VARIABLES GLOBALES Y FUNCIONES AUXILIARES ---

// 1. Obtenemos el <main> y guardamos su contenido HTML original
const mainContent = document.querySelector("#main-content");
const mainContentOriginalHTML = mainContent.innerHTML;

// 2. Función para activar el acordeón de películas (SOLO UNA DEFINICIÓN)
function activarAcordeonPeliculas() {
  const accordionItems = document.querySelectorAll(".accordion-item");
  if (accordionItems.length === 0) return;

  accordionItems.forEach((item) => {
    const header = item.querySelector(".accordion-header");
    if (header && !header.dataset.listenerAttached) {
      header.addEventListener("click", () => {
        const isActive = item.classList.contains("active");
        document.querySelectorAll(".accordion-item").forEach((otherItem) => {
          otherItem.classList.remove("active");
          otherItem.querySelector(".accordion-content").style.maxHeight = null;
        });
        if (!isActive) {
          item.classList.add("active");
          const content = item.querySelector(".accordion-content");
          content.style.maxHeight = content.scrollHeight + "px";
        }
      });
      header.dataset.listenerAttached = "true";
    }
    const form = item.querySelector(".admin-form");
    if (form && !form.dataset.listenerAttached) {
      form.addEventListener("submit", (e) => {
        e.preventDefault();
        mostrarVentana("Funcionalidad de API no implementada todavía.", "rojo");
      });
      form.dataset.listenerAttached = "true";
    }
  });
}

// ✅ REEMPLAZA ESTA FUNCIÓN
const cargarContenidoExterno = async (url) => {
  try {
    const respuesta = await fetch(url);
    if (!respuesta.ok) throw new Error("Recurso no encontrado");
    const html = await respuesta.text();
    mainContent.innerHTML = html;

    // --- LÓGICA DE ORQUESTACIÓN ---
    if (url === "pages/movie.html") {
      setTimeout(activarAcordeonPeliculas, 50);
      asignarListenerEliminar();
      asignarListenerActualizar();
      asignarListenerSubirImagen();
      asignarListenerRegistrar();
    } else if (url === 'pages/function.html') {
      inicializarVistaDetalle();
    } else if (url === 'pages/purchase_history.html') { // <-- LÍNEA AÑADIDA
      // Cuando se carga el historial, llama a la función para llenarlo.
      cargarHistorialDeCompras(); // <-- LÍNEA AÑADIDA
    }

  } catch (error) {
    console.error("Error al cargar la página:", error);
    mainContent.innerHTML = `<p style="text-align: center;">❌ Error al cargar el contenido.</p>`;
  }
};

// 4. Función para inicializar la página principal
function inicializarPaginaPrincipal() {
  cargarCarrusel();
  cargarPeliculas();
}
function convertirUrlYoutube(url) {
  // Si no hay URL, retorna una cadena vacía para evitar errores.
  if (!url) return "";

  const regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]*).*/;
  const match = url.match(regExp);

  // Si encuentra una coincidencia con un ID de video de 11 caracteres, construye la URL de embed.
  if (match && match[2].length === 11) {
    return `https://www.youtube.com/embed/${match[2]}`;
  } else {
    // Si no es una URL de YouTube válida, devuelve la original (no se mostrará el iframe).
    return url;
  }
}

// ✅ REEMPLAZA TU FUNCIÓN CON ESTA VERSIÓN PARA DEPURAR
let qrTimerInterval;
async function inicializarVistaDetalle() {
  const container = document.getElementById("detalle-pelicula-container");
  if (!container) return;

  try {
    const peliculaId = localStorage.getItem('peliculaIdSeleccionada');
    if (!peliculaId) throw new Error("No se ha seleccionado ninguna película.");

    const API_URL = `http://localhost:5235/api/Peliculas/obtenerporId?id=${peliculaId}`;
    const res = await fetch(API_URL);
    if (!res.ok) throw new Error("No se pudo encontrar la información de la película.");

    const pelicula = await res.json();
    const trailerUrl = convertirUrlYoutube(pelicula.trailer);

    container.innerHTML = `
      <div class="trailer-container">
        <iframe src="${trailerUrl}" title="Tráiler de ${pelicula.titulo}" frameborder="0" allowfullscreen></iframe>
      </div>
      <div class="info-container">
        <div class="sipnosis-container">
            <h1>${pelicula.titulo}</h1>
            <p>${pelicula.sipnosis}</p>
        </div>
        <div class="compra-container">
            <button class="boton-comprar">Comprar Boleto</button>
        </div>
      </div>
    `;

    // --- LÓGICA DE COMPRA, MODAL Y NAVEGACIÓN ---
    const comprarBoton = container.querySelector('.boton-comprar');
    const modal = document.getElementById('modal-qr');
    const closeButton = document.getElementById('modal-qr-close');
    const timerText = document.getElementById('modal-qr-timer');

    const cerrarModal = (compraExitosa = false) => {
      clearInterval(qrTimerInterval);
      modal.classList.remove('visible');
      if (compraExitosa) {
        cargarContenidoExterno('pages/purchase_history.html');
      }
    };

    const mostrarModal = () => {
      modal.classList.add('visible');
      let segundosRestantes = 10;
      timerText.textContent = `Cerrando en ${segundosRestantes} segundos...`;
      clearInterval(qrTimerInterval);
      qrTimerInterval = setInterval(() => {
        segundosRestantes--;
        timerText.textContent = `Cerrando en ${segundosRestantes} segundos...`;
        if (segundosRestantes <= 0) {
          cerrarModal(true);
        }
      }, 1000);
    };

    comprarBoton.addEventListener('click', async () => {
      const usuarioGuardado = localStorage.getItem("usuario");
      if (!usuarioGuardado) {
        mostrarVentana("Debes iniciar sesión para poder comprar.", "rojo");
        return;
      }

      const usuario = JSON.parse(usuarioGuardado);
      const idCuenta = usuario.idCuenta;
      const idPelicula = peliculaId;

      if (!idCuenta || !idPelicula) {
        mostrarVentana("Error de datos. Intenta iniciar sesión de nuevo.", "rojo");
        return;
      }

      try {
        const urlCompra = new URL("http://localhost:5235/api/Compras/Registrar");
        urlCompra.search = new URLSearchParams({ idCuenta, idPelicula });

        const response = await fetch(urlCompra, { method: "POST" });
        const result = await response.json();

        if (response.ok) {
          mostrarModal();
        } else {
          mostrarVentana(`Error al registrar la compra: ${result.message}`, "rojo");
        }
      } catch (error) {
        mostrarVentana("Error de red al intentar la compra.", "rojo");
      }
    });

    closeButton.addEventListener('click', () => cerrarModal(true));
    modal.addEventListener('click', (e) => {
      if (e.target === modal) {
        cerrarModal(true);
      }
    });

  } catch (err) {
    container.innerHTML = `<h1 style="color:red; text-align:center;">Error: ${err.message}</h1>`;
  }
}
// --- FUNCIÓN PRINCIPAL DE LOGIN ---

async function loginUsuario(e) {
  e.preventDefault();
  const email = document.getElementById("correoLogin").value.trim();
  const contrasena = document.getElementById("contrasenaLogin").value;
  if (!email || !contrasena) {
    mostrarVentana("Por favor completa todos los campos.", "rojo");
    return;
  }
  try {
    const url = new URL("http://localhost:5235/api/Cuentas/login");
    url.search = new URLSearchParams({ email, contrasena });
    const response = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/x-www-form-urlencoded" },
    });
    const result = await response.json();

    if (response.ok) {
      // Esta estructura leerá el 'idCuenta' que ahora SÍ envía el backend
      const usuario = {
        idCuenta: result.idCuenta, // Si el backend no envía 'idCuenta', esto se convierte en 'undefined'
        email: email,
        roles: result.roles
      };
      localStorage.setItem("usuario", JSON.stringify(usuario));

      // Actualiza la interfaz de usuario
      mostrarInterfazDeUsuarioLogueado(usuario);

      mostrarVentana("" + result.mensaje, "negro");
    } else {
      mostrarVentana((result.error || "Credenciales incorrectas"), "rojo");
    }
  } catch (error) {
    console.error(error);
    mostrarVentana("No se pudo conectar con el servidor.", "rojo");
  }
}
// Espera a que el DOM esté completamente cargado para ejecutar el script
window.addEventListener("DOMContentLoaded", cargarCarrusel);

async function cargarCarrusel() {
  const API_URL = "http://localhost:5235/api/Peliculas/obtener";

  const carrusel = document.getElementById("carrusel");
  const indicadoresContainer = document.getElementById("indicadores");
  const prevBtn = document.getElementById("prevBtn");
  const nextBtn = document.getElementById("nextBtn");

  // Limpiar contenido previo para evitar duplicados si se llama de nuevo
  carrusel.innerHTML = '';
  indicadoresContainer.innerHTML = '';

  try {
    const res = await fetch(API_URL);
    if (!res.ok) {
      throw new Error(`Error al conectar con la API: ${res.status}`);
    }
    const peliculas = await res.json();

    if (!peliculas || peliculas.length === 0) {
      carrusel.innerHTML =
        "<p style='text-align:center;color:white;width:100%;'>No hay películas disponibles.</p>";
      return;
    }

    let currentIndex = 0;

    // Crear un slide por cada película
    peliculas.forEach((pelicula, i) => {
      // 1. Crear el contenedor del slide
      const slide = document.createElement("div");
      slide.className = "slide";

      // --- CAMBIO PRINCIPAL: Usamos una etiqueta <img> para la imagen ---
      // Esto es mejor para la responsividad, el rendimiento (lazy loading) y la accesibilidad (alt text).
      const img = document.createElement("img");
      img.src = pelicula.imagenHorizontal || `https://via.placeholder.com/1600x900?text=${encodeURIComponent(pelicula.titulo)}`;
      img.alt = pelicula.titulo;
      img.loading = "lazy"; // Mejora el rendimiento: carga la imagen solo cuando está cerca de verse
      slide.appendChild(img);
      // -------------------------------------------------------------------
      if (pelicula.imagenFoco) {
        img.style.objectPosition = pelicula.imagenFoco;
      }
      // 2. Crear el contenedor para la información
      const info = document.createElement("div");
      info.className = "info";

      // 3. Crear y añadir el título
      const titulo = document.createElement("h2");
      titulo.className = "pelicula-titulo";
      titulo.textContent = pelicula.titulo || "Título no disponible";
      info.appendChild(titulo);

      // 4. Crear y añadir los botones
      const botones = document.createElement("div");
      botones.className = "botones";
      botones.innerHTML = `
        <button class="btn-trailer">
            <span class="play-icon">▶</span> Ver Trailer
        </button>
        <button class="btn-detalle">Detalle</button>
      `;
      info.appendChild(botones);

      // 5. Añadir la información (encima de la imagen) al slide y el slide al carrusel
      slide.appendChild(info);
      carrusel.appendChild(slide);

      // 6. Crear el indicador para este slide
      const indicador = document.createElement("span");
      if (i === 0) indicador.classList.add("activo");
      indicador.addEventListener("click", () => {
        currentIndex = i;
        moverCarrusel();
      });
      indicadoresContainer.appendChild(indicador);
    });

    const totalSlides = peliculas.length;

    // Función para actualizar la posición del carrusel y el indicador activo
    function moverCarrusel() {
      carrusel.style.transform = `translateX(-${currentIndex * 100}%)`;

      const puntos = indicadoresContainer.querySelectorAll("span");
      puntos.forEach((p, i) =>
        p.classList.toggle("activo", i === currentIndex)
      );
    }

    // Event listeners para los botones de navegación
    nextBtn.addEventListener("click", () => {
      currentIndex = (currentIndex + 1) % totalSlides;
      moverCarrusel();
    });

    prevBtn.addEventListener("click", () => {
      currentIndex = (currentIndex - 1 + totalSlides) % totalSlides;
      moverCarrusel();
    });

    // Deslizamiento automático cada 5 segundos
    const autoSlide = setInterval(() => {
      currentIndex = (currentIndex + 1) % totalSlides;
      moverCarrusel();
    }, 5000);

  } catch (error) {
    console.error("Error al cargar el carrusel:", error);
    carrusel.innerHTML =
      "<p style='color:red;text-align:center;width:100%;'>Error al cargar las películas desde la API 😢</p>";
  }
}
// ✅ REEMPLAZA ESTA FUNCIÓN CON LA VERSIÓN MEJORADA
async function cargarHistorialDeCompras() {
  const container = document.getElementById("historial-container");
  if (!container) return;

  // 1. Obtener el idCuenta del usuario logueado (sin cambios)
  const usuarioGuardado = localStorage.getItem("usuario");
  if (!usuarioGuardado) {
    container.innerHTML = "<h2>Debes iniciar sesión para ver tu historial.</h2>";
    return;
  }
  const usuario = JSON.parse(usuarioGuardado);
  const idCuenta = usuario.idCuenta;

  try {
    // 2. Obtener la lista de compras del historial (sin cambios)
    const urlHistorial = new URL("http://localhost:5235/api/Compras/Historial");
    urlHistorial.search = new URLSearchParams({ idCuenta });
    const responseHistorial = await fetch(urlHistorial);

    if (responseHistorial.status === 404) {
      container.innerHTML = "<h2>Aún no has realizado ninguna compra.</h2>";
      return;
    }
    if (!responseHistorial.ok) throw new Error("No se pudo cargar el historial.");

    const historial = await responseHistorial.json();
    container.innerHTML = ""; // Limpiar el "Cargando..."

    // --- ¡AQUÍ ESTÁ LA NUEVA LÓGICA! ---

    // 3. Creamos un array de "promesas". Cada promesa será una llamada fetch
    //    para obtener los detalles de una película.
    const promesasDePeliculas = historial.map(compra => {
      const urlPelicula = `http://localhost:5235/api/Peliculas/obtenerParaAdmin?id=${compra.idPelicula}`;
      return fetch(urlPelicula).then(res => {
        // Si la película no se encuentra, devolvemos un objeto por defecto
        if (!res.ok) return { titulo: 'Película no encontrada', imagenVertical: null };
        return res.json();
      });
    });

    // 4. Usamos Promise.all para esperar a que TODAS las llamadas fetch terminen.
    //    Esto es muy rápido porque se ejecutan en paralelo.
    const detallesDePeliculas = await Promise.all(promesasDePeliculas);

    // 5. Ahora, combinamos los datos y creamos las tarjetas.
    historial.forEach((compra, index) => {
      const pelicula = detallesDePeliculas[index]; // Obtenemos la película correspondiente
      const fecha = new Date(compra.fecha).toLocaleDateString('es-ES');

      const card = document.createElement('div');
      card.className = 'compra-card';
      card.innerHTML = `
        <div class="img-container">
          <img src="${pelicula?.imagenVertical || 'assets/img/placeholder.png'}" alt="Portada de ${pelicula?.titulo}">
        </div>
        <div class="compra-info">
          <h3>${pelicula?.titulo || 'Película no disponible'}</h3>
          <p><strong>Fecha de compra:</strong> ${fecha}</p>
          <p><strong>Total:</strong> ${compra.total.toFixed(2)} Bs.</p>
        </div>
      `;
      container.appendChild(card);
    });

  } catch (error) {
    console.error("Error al cargar el historial:", error);
    container.innerHTML = "<h2>❌ Error al cargar el historial.</h2>";
  }
}

// En api.js

// ✅ REEMPLAZA ESTA FUNCIÓN TAMBIÉN PARA EVITAR CONFLICTOS
function activarAcordeonPeliculas() {
  const accordionItems = document.querySelectorAll(".accordion-item");
  if (accordionItems.length === 0) return;

  accordionItems.forEach((item) => {
    const header = item.querySelector(".accordion-header");
    if (header && !header.dataset.listenerAttached) {
      header.addEventListener("click", () => {
        const isActive = item.classList.contains("active");
        document.querySelectorAll(".accordion-item").forEach((otherItem) => {
          otherItem.classList.remove("active");
          otherItem.querySelector(".accordion-content").style.maxHeight = null;
        });
        if (!isActive) {
          item.classList.add("active");
          const content = item.querySelector(".accordion-content");
          content.style.maxHeight = content.scrollHeight + "px";
        }
      });
      header.dataset.listenerAttached = "true";
    }

    // Corregimos el listener conflictivo para que ignore formularios con ID
    const form = item.querySelector(".admin-form:not([id])");
    if (form && !form.dataset.listenerAttached) {
      form.addEventListener("submit", (e) => {
        e.preventDefault();
      });
      form.dataset.listenerAttached = "true";
    }
  });
}
// En tu archivo api.js, añade esta nueva función
/**
 * Asigna un event listener al formulario de registro de películas.
 * Al enviar el formulario, recopila los datos, valida y los envía a la API.
 */
function asignarListenerRegistrar() {
  const formRegistrar = document.getElementById("form-registrar");

  if (formRegistrar) {
    formRegistrar.addEventListener("submit", async (e) => {
      e.preventDefault();

      const titulo = document.getElementById("reg-titulo").value.trim();
      const duracion = document.getElementById("reg-duracion").value;
      const genero = document.getElementById("reg-genero").value.trim();
      const fechaEstreno = document.getElementById("reg-fecha").value;
      const restriccionEdad = document.getElementById("reg-restriccion").value.trim();
      const sipnosis = document.getElementById("reg-sipnosis").value.trim();

      if (!titulo || !duracion || !genero || !fechaEstreno || !restriccionEdad || !sipnosis) {
        mostrarVentana("Por favor, completa todos los campos.", "rojo");
        return;
      }

      const url = new URL("http://localhost:5235/api/Peliculas/registrar");

      // --- SOLUCIÓN AQUÍ ---
      // Añadimos el campo 'estado' que el backend requiere.
      const params = new URLSearchParams({
        titulo,
        duracion,
        genero,
        fechaEstreno,
        restriccionEdad,
        sipnosis,
        estado: "activo", // <--- LÍNEA AÑADIDA
      });
      url.search = params.toString();

      try {
        const response = await fetch(url, {
          method: "POST",
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
          },
        });

        const result = await response.json();

        if (response.ok) {
          mostrarVentana(`${result.mensaje}`, "negro");
          e.target.reset();
        } else {
          const errorMsg = result.mensaje || JSON.stringify(result);
          mostrarVentana(` Error del servidor: ${errorMsg}`, "rojo");
        }
      } catch (error) {
        mostrarVentana("Error de red o conexión: " + error.message, "rojo");
      }
    });
  }
}
function verificarSesionAlCargar() {
  const usuarioGuardado = localStorage.getItem("usuario");

  // Si no hay nada en localStorage, no hacemos nada.
  if (!usuarioGuardado) {
    return;
  }

  try {
    const usuario = JSON.parse(usuarioGuardado);

    // --- ¡AQUÍ ESTÁ LA VALIDACIÓN CLAVE! ---
    // Verificamos que el objeto 'usuario' tenga las propiedades 'email' y 'roles'.
    if (usuario && usuario.email && usuario.roles) {
      // Si el objeto es válido, procedemos a mostrar la interfaz.
      mostrarInterfazDeUsuarioLogueado(usuario);
    } else {
      // Si el objeto es inválido (le falta email o roles), lo consideramos corrupto.
      console.warn("Se encontró un objeto de usuario inválido en localStorage. Se limpiará la sesión.");
      // Borramos los datos malos para que no vuelvan a dar problemas.
      localStorage.removeItem("usuario");
    }
  } catch (error) {
    // Si el JSON está tan mal que ni se puede parsear, también lo borramos.
    console.error("Error al leer datos de sesión. Se limpiará la sesión.", error);
    localStorage.removeItem("usuario");
  }
}
function asignarListenerEliminar() {
  const formEliminar = document.getElementById("form-eliminar");
  if (!formEliminar) return;

  formEliminar.addEventListener("submit", async (e) => {
    e.preventDefault();

    const titulo = document.getElementById("del-titulo").value.trim();
    if (!titulo) {
      mostrarVentana("Por favor ingresa un título válido.", "rojo");
      return;
    }

    try {
      // ✅ CAMBIO: Construimos la URL con el parámetro 'titulo'
      // Usamos encodeURIComponent para manejar títulos con espacios o caracteres especiales
      const url = `http://localhost:5235/api/Peliculas/eliminar?titulo=${encodeURIComponent(titulo)}`;

      const response = await fetch(url, {
        method: "DELETE",
      });

      if (response.ok) {
        const data = await response.json();
        mostrarVentana(data.mensaje || "Película eliminada correctamente.", "negro");
        formEliminar.reset();
      } else {
        const error = await response.json();
        mostrarVentana(error.mensaje || "No se pudo eliminar la película.", "rojo");
      }
    } catch (err) {
      // console.error("Error al eliminar la película:", err);
      mostrarVentana("Error al conectar con el servidor.", "rojo");
    }
  });
}
function asignarListenerActualizar() {
  const formActualizar = document.getElementById("form-actualizar");
  if (!formActualizar) return;

  formActualizar.addEventListener("submit", async (e) => {
    e.preventDefault();
    const tituloActual = document
      .getElementById("act-titulo-actual")
      .value.trim();
    const nuevoTitulo = document
      .getElementById("act-nuevo-titulo")
      .value.trim();
    if (!tituloActual || !nuevoTitulo) {
      mostrarVentana("Por favor ingresa el título actual y el nuevo título.", "rojo");
      return;
    }
    try {
      // Construimos la URL de la API con los parámetros del query string
      const url = new URL("http://localhost:5235/api/Peliculas/actualizar");
      url.search = new URLSearchParams({
        tituloActual,
        nuevoTitulo,
      });

      const response = await fetch(url.toString(), {
        method: "PUT",
        headers: {
          Accept: "application/json",
        },
      });

      if (!response.ok) {
        const error = await response.json();
        mostrarVentana(error.mensaje || "No se pudo actualizar la película.", "rojo");
        return;
      }

      const data = await response.json();
      mostrarVentana(data.mensaje || "Película actualizada correctamente.", "negro");
      formActualizar.reset();
    } catch (err) {
      mostrarVentana(
        "Error al conectar con el servidor. Verifica que la API esté en ejecución."
        , "rojo");
    }
  });
}

async function asignarListenerSubirImagen() {
  const form = document.getElementById("form-subir-imagen");
  if (!form) return;

  form.addEventListener("submit", async (e) => {
    e.preventDefault();

    // 1. Obtener valores de TODOS los campos
    const titulo = document.getElementById("img-titulo").value.trim();
    const archivoVertical = document.getElementById("img-vertical").files[0]; // Archivo 1
    const archivoHorizontal = document.getElementById("img-horizontal").files[0]; // Archivo 2
    const trailer = document.getElementById("trailer-url").value.trim(); // Trailer

    // 2. Validar que todos los campos estén completos
    if (!titulo || !archivoVertical || !archivoHorizontal || !trailer) {
      mostrarVentana("Por favor, completa todos los campos para subir los archivos.", "negro");
      return;
    }

    try {
      // 3. Crear el FormData y AÑADIR TODOS los parámetros
      const formData = new FormData();
      // Los nombres (primer parámetro de append) DEBEN coincidir con los de la API
      formData.append("titulo", titulo);
      formData.append("archivo1", archivoVertical);       // Coincide con IFormFile archivo1
      formData.append("archivo2", archivoHorizontal);     // Coincide con IFormFile archivo2
      formData.append("trailer", trailer);                // Coincide con string trailer

      // Enviar petición al backend (la URL y el método no cambian)
      const response = await fetch(
        "http://localhost:5235/api/Peliculas/subirImagen",
        {
          method: "POST",
          body: formData, // El body ahora contiene todos los datos
        }
      );

      // Leer respuesta (sin cambios en esta parte)
      const text = await response.text();
      console.log("Status:", response.status);
      console.log("Body:", text);

      if (response.ok) {
        const data = JSON.parse(text);
        mostrarVentana(data.mensaje || "Archivos subidos correctamente.", "negro");
        form.reset();
      } else {
        try {
          const error = JSON.parse(text);
          mostrarVentana(error.mensaje || "No se pudieron subir los archivos.", "rojo");
        } catch {
          mostrarVentana(" Error inesperado: " + text, "rojo");
        }
      }
    } catch (err) {
      mostrarVentana(
        "No se pudo conectar con el servidor. Verifica que la API esté ejecutándose."
        , "rojo");
    }
  });
}
// ✅ VERSIÓN ACTUALIZADA - Copia y pega esta función completa

async function cargarPeliculas() {
  const API_URL = "http://localhost:5235/api/Peliculas/obtener";
  try {
    const res = await fetch(API_URL);
    if (!res.ok) throw new Error("La respuesta de la API no fue exitosa.");

    const peliculas = await res.json();
    const container = document.getElementById("peliculasContainer");
    container.innerHTML = ""; // Limpiamos el contenedor

    peliculas.forEach((pelicula) => {
      const div = document.createElement("div");
      div.className = "pelicula-card";
      div.innerHTML = `
        <img src="${pelicula.imagenVertical || "https://via.placeholder.com/220x300?text=Sin+Imagen"}" 
             alt="${pelicula.titulo}">
        <div class="pelicula-info">
            <h3>${pelicula.titulo}</h3>                    
        </div>`;

      // --- ¡ESTE ES EL CAMBIO MÁS IMPORTANTE! ---
      // Al hacer clic en una película:
      div.addEventListener('click', () => {
        // 1. Guardamos el ID de la película seleccionada en el almacenamiento local del navegador.
        //    Esto actúa como un "mensaje" para la siguiente vista.
        localStorage.setItem('peliculaIdSeleccionada', pelicula.idPelicula);

        // 2. Usamos tu función de SPA para cargar el contenido de 'function.html'
        //    sin recargar toda la página.
        cargarContenidoExterno('pages/function.html');
      });

      container.appendChild(div);
    });
  } catch (err) {
    document.getElementById("peliculasContainer").innerHTML =
      "<p style='color:red;text-align:center;'>No se pudieron cargar las películas 😢</p>";
  }
}
function mostrarVentana(mensaje, color) {
  const notificacion = document.createElement('div');
  notificacion.textContent = mensaje;

  Object.assign(notificacion.style, {
    position: 'fixed',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    color: 'white',
    padding: '15px 25px',
    borderRadius: '10px',
    fontSize: '18px',
    textAlign: 'center',
    zIndex: '9999',
    opacity: '0',
    transition: 'opacity 0.5s ease',
    backgroundColor: color === 'rojo' ? '#e74c3c' : '#2c3e50',
  });

  document.body.appendChild(notificacion);

  setTimeout(() => notificacion.style.opacity = '1', 10);
  setTimeout(() => {
    notificacion.style.opacity = '0';
    setTimeout(() => notificacion.remove(), 500);
  }, 3000);
}