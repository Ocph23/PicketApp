namespace SharedModel
{
    public enum Gender
    {
        Pria,
        Wanita
    }

    public enum Weather
    {
        Cerah,
        Mendung,
        Gerimis,
        Hujan,
    }


    public enum AttendanceStatus
    {
        None,
        Hadir,
        Terlambat,
        Alpa,
        Sakit,
        Izin,
        Bolos,
        Lainnya
    }

    public enum LateAndGoHomeEarlyAttendanceStatus
    {
        Terlambat,
        Pulang,
    }

    public enum StudentStatus
    {
        TanpaStatus = 0,
        Aktif = 1,
        Pindah,
        Tamat,
        Keluar,
    }


    public enum StudentProgressNoteType
    {
        Informasi,
        Prestasi,
        Pelanggaran,
        Lainnya
    }
}
