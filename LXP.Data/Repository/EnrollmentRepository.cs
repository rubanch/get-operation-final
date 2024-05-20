using LXP.Common.Entities;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
   
    
        public class EnrollmentRepository : IEnrollmentRepository
        {
            private readonly LXPDbContext _lXPDbContext;

            public EnrollmentRepository(LXPDbContext lXPDbContext)
            {
               this. _lXPDbContext = lXPDbContext;
            }

            public async Task  Addenroll(Enrollment enrollment)
            {
                await _lXPDbContext.Enrollments.AddAsync(enrollment);
                await _lXPDbContext.SaveChangesAsync();

            }

            public bool AnyEnrollmentByLearnerAndCourse(Guid LearnerId, Guid CourseId )
            {
                return _lXPDbContext.Enrollments.Any(enrollment => enrollment.LearnerId == LearnerId && enrollment.CourseId == CourseId);
            }

              public object GetCourseandTopicsByLearnerId(Guid learnerId)
  {
      var result = from enrollment in _lXPDbContext.Enrollments
                   where enrollment.LearnerId == learnerId
                   select new
                   {
                       enrolledCourseId = enrollment.CourseId,
                       enrolledCoursename=enrollment.Course.Title,
                       enrolledcoursedescription = enrollment.Course.Description,
                       enrolledcoursecategory = enrollment.Course.Catagory.Category,
                       enrolledcourselevels = enrollment.Course.Level.Level,
                       Thumbnailimage = String.Format("{0}://{1}{2}/wwwroot/CourseThumbnailImages/{3}",
                       _contextAccessor.HttpContext.Request.Scheme,
                       _contextAccessor.HttpContext.Request.Host,
                       _contextAccessor.HttpContext.Request.PathBase,
                       enrollment.Course.Thumbnail),

                       Topics = (from topic in _lXPDbContext.Topics
                                 where topic.CourseId == enrollment.CourseId && topic.IsActive == true
                                 select new
                                 {
                                     TopicName = topic.Name,
                                     TopicDescription = topic.Description,
                                     TopicId = topic.TopicId,
                                     TopicIsActive = topic.IsActive,


                                 }).ToList()
                   };
      return result;


  }
        }
    }

