using MVCWithREST.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MVCWithREST.Controllers
{
    public class NotesController : Controller
    {
        HttpClient client = new HttpClient();

        public NotesController()
        {
            client.BaseAddress = new Uri("http://devmedianotesapi.azurewebsites.net");
            client.DefaultRequestHeaders.Accept.Clear(); //exclui todos os headers que vem como padrão
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); ///adiciono o tipo json
        }

        // GET: Notes
        public ActionResult Index()
        {
            List<Note> notes = new List<Note>();
            //faz o get mudando o final do endereço e pega o resultado
            HttpResponseMessage response = client.GetAsync("/api/notes").Result;
            //se for 200 OK popula a lista de notas com o resultado da requisição
            if (response.IsSuccessStatusCode)
                notes = response.Content.ReadAsAsync<List<Note>>().Result;

            return View(notes);
        }

        // GET: Notes/Details/5
        public ActionResult Details(int id)
        {
            //$ string interpolation - dentro dela conseguimos encaixar o valor que vem como parametro
            HttpResponseMessage response = client.GetAsync($"/api/notes/{id}").Result;
            Note note = response.Content.ReadAsAsync<Note>().Result;
            if (note != null)
                return View(note);
            else
                return HttpNotFound(); //retorna 404
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        [HttpPost]
        public ActionResult Create(Note note)
        {
            try
            {
                HttpResponseMessage response = client.PostAsJsonAsync<Note>("/api/notes", note).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.Created) //http 201
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "Erro durante a criação da nota";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/notes/{id}").Result;
            Note note = response.Content.ReadAsAsync<Note>().Result;
            if (note != null)
                return View(note);
            else
                return HttpNotFound(); //retorna 404
        }

        // POST: Notes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Note note)
        {
            try
            {
                HttpResponseMessage response = client.PutAsJsonAsync<Note>($"/api/notes/{id}", note).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent) //http 201
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "Erro durante a criação da nota";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = client.GetAsync($"/api/notes/{id}").Result;
            Note note = response.Content.ReadAsAsync<Note>().Result;
            if (note != null)
                return View(note);
            else
                return HttpNotFound(); //retorna 404
        }

        // POST: Notes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Note note)
        {
            try
            {
                HttpResponseMessage response = client.DeleteAsync($"/api/notes/{id}").Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK) //http 200
                    return RedirectToAction("Index");
                else
                {
                    ViewBag.Error = "Erro durante a criação da nota";
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
