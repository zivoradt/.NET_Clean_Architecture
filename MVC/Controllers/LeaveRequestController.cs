using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Contracts;
using MVC.Models;
using NuGet.Protocol;
using System.Reflection;

namespace MVC.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IMapper _mapper;

        public LeaveRequestController(ILeaveTypeService leaveTypeService,
            ILeaveRequestService leaveRequestService,
            IMapper mapper)
        {
            _leaveTypeService = leaveTypeService;
            _leaveRequestService = leaveRequestService;
            _mapper = mapper;
        }

        // GET: LeaveRequestController/Create
        public async Task<ActionResult> Create()
        {
            var leaveTypes = await _leaveTypeService.GetAllTypes();

            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");

            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypeItems,
            };
            return View(model);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateLeaveRequestVM createLeaveRequestVM)
        {
            if (!ModelState.IsValid && ModelState.ErrorCount == 1 && ModelState.ContainsKey("LeaveTypes"))
            {
                var response = await _leaveRequestService.CreateLeaveRequest(createLeaveRequestVM);

                if (response.Success)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (!string.IsNullOrEmpty(response.ValidationErrors))
                {
                    ModelState.AddModelError("", response.ValidationErrors);
                }
            }

            var leaveTypes = await _leaveTypeService.GetAllTypes();
            var leaveTypeItems = new SelectList(leaveTypes, "Id", "Name");

            createLeaveRequestVM.LeaveTypes = leaveTypeItems;

            return View(createLeaveRequestVM);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Index()
        {
            Console.WriteLine("LeaveRequest");
            var model = await _leaveRequestService.GetAdminLeaveRequestList();
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var model = await _leaveRequestService.GetLeaveRequest(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> ApproveRequest(int id, bool approved)
        {
            try
            {
                await _leaveRequestService.ApproveLeaveRequest(id, approved);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: LeaveRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public static void PrintObjectProperties(object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Object is null");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj, null);
                Console.WriteLine($"{property.Name}: {value}");
            }
        }
    }
}