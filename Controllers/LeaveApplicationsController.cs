﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
			var awatingstatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "AwaitingApproval").FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
				.Where(l => l.StatusId == awatingstatus!.Id);
			return View(await applicationDbContext.ToListAsync());
        }

		public async Task<IActionResult> ApprovedLeaveApplications()
		{
			var approvestatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved").FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusId == approvestatus!.Id);
			return View(await applicationDbContext.ToListAsync());
		}

		public async Task<IActionResult> RejectedLeaveApplications()
		{
			var rejectedtatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected").FirstOrDefault();

			var applicationDbContext = _context.LeaveApplications
				.Include(l => l.Duration)
				.Include(l => l.Employee)
				.Include(l => l.LeaveType)
				.Include(l => l.Status)
				.Where(l => l.StatusId == rejectedtatus!.Id);
			return View(await applicationDbContext.ToListAsync());
		}


		// GET: LeaveApplications/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x=>x.SystemCode).Where(y=>y.SystemCode.Code=="LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
          /*  ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description");*/
            return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        {
			//var pendingStatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus").FirstOrDefaultAsync();
			var pendingStatus =await _context.SystemCodeDetails.Include(x => x.SystemCode)
		.FirstOrDefaultAsync(y => y.Code == "AwaitingApproval" && y.SystemCode.Code == "LeaveApprovalStatus");

			ModelState.Remove("Employee");
			ModelState.Remove("Status");
			ModelState.Remove("Duration");
			ModelState.Remove("LeaveType");
			if (ModelState.IsValid)
            {
                leaveApplication.CreatedById = "Pranay";
                leaveApplication.CreatedOn = DateTime.Now;
                leaveApplication.StatusId = pendingStatus.Id;
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x=>x.SystemCode).Where(y=>y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
           //ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description", leaveApplication.StatusId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
          //ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description", leaveApplication.StatusId);
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
				var pendingStatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus").FirstOrDefaultAsync();
				//var pendingStatus = await _context.SystemCodeDetails.Include(x => x.SystemCode).FirstOrDefaultAsync(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus");
				try
				{
                    leaveApplication.StatusId = pendingStatus.Id;
                    leaveApplication.ModifiedOn = DateTime.Now;
                    leaveApplication.ModifiedById = "Pranay";
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeId);
         //   ViewData["StatusId"] = new SelectList(_context.SystemCodeDetails, "Id", "Description", leaveApplication.StatusId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }

        [HttpGet]
		public async Task<IActionResult> ApproveLeave(int? id)
		{
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
				.Include(l => l.Employee)
				.Include(l => l.LeaveType)
				.Include(l => l.Status)
				.FirstOrDefaultAsync(y => y.Id == id);

			if (leaveApplication == null)
			{
				
				return NotFound();
			}
			ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
			ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName");
			ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
			return View(leaveApplication);
		}

		[HttpPost]
		public async Task<IActionResult> ApproveLeave(LeaveApplication leave)
		{
            var approvestatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code=="Approved").FirstOrDefault();

			var leaveApplication = await _context.LeaveApplications

				.Include(l => l.Duration)
				.Include(l => l.Employee)
				.Include(l => l.LeaveType)
				.Include(l => l.Status)
				.FirstOrDefaultAsync(y => y.Id == leave.Id);

			if (leaveApplication == null)
			{

				return NotFound();
			}
            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Pranay";
			leaveApplication.ModifiedOn = DateTime.Now;
			leaveApplication.ModifiedById = "Pranay";
            leaveApplication.StatusId = approvestatus!.Id;
            leaveApplication.ApprovalNotes = leave.ApprovalNotes;


			_context.Update(leaveApplication);
            await _context.SaveChangesAsync();

			ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
			ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName");
			ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> RejectLeave(int? id)
		{
			var leaveApplication = await _context.LeaveApplications
				.Include(l => l.Duration)
				.Include(l => l.Employee)
				.Include(l => l.LeaveType)
				.Include(l => l.Status)
				.FirstOrDefaultAsync(y => y.Id == id);

			if (leaveApplication == null)
			{

				return NotFound();
			}
			ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
			ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName");
			ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
			return View(leaveApplication);
		}

		[HttpPost]
		public async Task<IActionResult> RejectLeave(LeaveApplication leave)
		{
			var rejectstatus = _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected").FirstOrDefault();

			var leaveApplication = await _context.LeaveApplications
				.Include(l => l.Duration)
				.Include(l => l.Employee)
				.Include(l => l.LeaveType)
				.Include(l => l.Status)
				.FirstOrDefaultAsync(y => y.Id == leave.Id);

			if (leaveApplication == null)
			{

				return NotFound();
			}
			leaveApplication.ApprovedOn = DateTime.Now;
			leaveApplication.ApprovedById = "Pranay";
			leaveApplication.ModifiedOn = DateTime.Now;
			leaveApplication.ModifiedById = "Pranay";
			leaveApplication.StatusId = rejectstatus!.Id;
			leaveApplication.ApprovalNotes = leave.ApprovalNotes;

			_context.Update(leaveApplication);
			await _context.SaveChangesAsync();

			ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
			ViewData["EmployeeId"] = new SelectList(_context.employees, "Id", "FullName");
			ViewData["LeaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
			return RedirectToAction(nameof(Index));
		}



	}
}
