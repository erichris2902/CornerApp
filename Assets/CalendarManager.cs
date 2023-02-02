using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    public int day;
    public int month;
    public int year;

    public string s_month;

    public int total_day = 0;
    public int total_month = 0;
    public int total_day_pendientes= 0;
    public int total_last_month = 0;
    public int total_two_months_ago = 0;

    public List<ZonaInfo> infoZone = new List<ZonaInfo>();

    public Dictionary<int, int> month_days = new Dictionary<int, int>();

    public void Awake()
    {
        month_days.Add(1, 31);
        month_days.Add(2, 28);
        month_days.Add(3, 31);
        month_days.Add(4, 30);
        month_days.Add(5, 31);
        month_days.Add(6, 30);
        month_days.Add(7, 31);
        month_days.Add(8, 31);
        month_days.Add(9, 30);
        month_days.Add(10, 31);
        month_days.Add(11, 30);
        month_days.Add(12, 31);
    }

    public MyDate GetYesterday()
    {
        MyDate myDate = new MyDate();

        if (day == 1)
        {
            if(month == 1)
            {
                //date_yestardary = (month_days[month - 1]).ToString() + "/" + (12).ToString() + "/" + (year-1).ToString();
                myDate.day = month_days[month - 1];
                myDate.month = 12;
                myDate.year = year - 1;
            }
            else
            {
                //date_yestardary = (month_days[month - 1]).ToString() + "/" + (month-1).ToString() + "/" + year.ToString();
                myDate.day = month_days[month - 1];
                myDate.month = month - 1;
                myDate.year = year;
            }
        }
        else{
            //date_yestardary = (day-1).ToString() + "/" + month.ToString() + "/" + year.ToString();
            myDate.day = day - 1;
            myDate.month = month;
            myDate.year = year;
        }

        day = myDate.day;
        month = myDate.month;
        year = myDate.year;

        return myDate;
    }

    public MyDate GetTomorrow()
    {
        MyDate myDate = new MyDate();

        if (day == month_days[month])
        {
            if (month == 12)
            {
                //date_tomorrow = (1).ToString() + "/" + (1).ToString() + "/" + (year + 1).ToString();
                myDate.day = 1;
                myDate.month = 1;
                myDate.year = year + 1;
            }
            else
            {
                //date_tomorrow = (1).ToString() + "/" + (month + 1).ToString() + "/" + year.ToString();
                myDate.day = 1;
                myDate.month = month + 1;
                myDate.year = year;
            }
        }
        else
        {
            //date_tomorrow = (day + 1).ToString() + "/" + month.ToString() + "/" + year.ToString();
            myDate.day = day + 1;
            myDate.month = month;
            myDate.year = year;
        }

        day = myDate.day;
        month = myDate.month;
        year = myDate.year;

        return myDate;
    }
}


[Serializable]
public class ZonaInfo
{
    public string name = "";
    public string day = "";
    public string month = "";
}


[Serializable]
public class MyDate
{
    public int day = 0;
    public int month = 0;
    public int year = 0;
}