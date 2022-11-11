cmake_minimum_required(VERSION 3.0.0)
project(amgcl VERSION 0.1.0)

include(CTest)
enable_testing()

include_directories(
  ~/sources/amgcl-1.4.3
  ~/sources/boost-1.74.0
  )

add_library(amgcl SHARED amgcl.cpp)

set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)
