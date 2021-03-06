﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationService;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Repository;

namespace Web.Controllers
{
    public class AlunoController : Controller
    {
        private AlunoServices Services { get; set; }

        public AlunoController(AlunoServices services)
        {
            this.Services = services;
        }

        // GET: AlunoController
        public ActionResult Index()
        {
            if (this.HttpContext.Session.GetString("AlunosSelecionados") != null)
            {
                var idsSelecionados = JsonConvert.DeserializeObject<List<Aluno>>(this.HttpContext.Session.GetString("AlunosSelecionados"));
                ViewBag.IdsSelecionados = idsSelecionados.Select(x => x.Id.ToString());
            }
                    

            return View(this.Services.GetAll());
        }

        public ActionResult AlunosSelecionados(string ids)
        {
            List<Aluno> alunosSelecionados = new List<Aluno>();

            this.HttpContext.Session.Clear();

            if (!String.IsNullOrWhiteSpace(ids))
            {
                foreach (var item in ids.Split(","))
                {
                    alunosSelecionados.Add(this.Services.GetAlunoById(new Guid(item)));
                }
            }

            this.HttpContext.Session.SetString("AlunosSelecionados", JsonConvert.SerializeObject(alunosSelecionados));

            return View(alunosSelecionados);
        }

        // GET: AlunoController/Details/5
        public ActionResult Details(Guid id)
        {
            var aluno = this.Services.GetAlunoById(id);

            return View(aluno);
        }

        // GET: AlunoController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var aluno = this.Services.GetAlunoById(id);
            return View(aluno);
        }

        // POST: AlunoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Aluno aluno)
        {
            try
            {
                if (ModelState.IsValid == false)
                    return View(aluno);

                this.Services.Save(aluno);

                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("APP_ERROR", ex.Message);
                return View(aluno);
            }
        }

        // GET: AlunoController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var aluno = this.Services.GetAlunoById(id);

            return View(aluno);
        }

        // POST: AlunoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, Aluno aluno)
        {
            try
            {
                this.Services.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



    }
}
